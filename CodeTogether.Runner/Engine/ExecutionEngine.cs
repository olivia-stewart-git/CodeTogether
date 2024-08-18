using CodeTogether.Data.Models.Game;
using CodeTogether.Data.Models.Questions;
using CodeTogether.Data.Models.Submission;
using CodeTogether.Runner.Adaptors;

namespace CodeTogether.Runner.Engine;

public class ExecutionEngine : IExecutionEngine
{
	readonly ICompilationEngine compilationEngine;
	readonly IExecutorFactory executorFactory;

	public ExecutionEngine(ICompilationEngine compilationEngine, IExecutorFactory executorFactory)
	{
		this.compilationEngine = compilationEngine;
		this.executorFactory = executorFactory;
	}

	public SubmissionModel ExecuteAgainstQuestion(QuestionModel question, string code, GamePlayerModel submitter)
	{

		var configuration = question.QST_Scaffold;
		var compilationName = $"Compilation_{question.QST_Name}{Guid.NewGuid()}";
		
		var executor = executorFactory.GetExecutor(configuration, question.QST_TestCases)
			?? throw new ExecutionSetupException($"Could not resolve an executor for {configuration.EXE_ExecutionRunnerName}");

		var parameterTypes = question.QST_Scaffold.EXE_Parameters.Select(p => p.TC_Type.OT_Type ?? throw new InvalidOperationException("Scaffold should not have invalid parameter types"));
		var returnType = question.QST_Scaffold.EXE_ReturnType.OT_Type ?? throw new InvalidOperationException("Scaffold should not have invalid return type");
		var typesReferences = parameterTypes.Append(returnType).Where(t => !t.Namespace.StartsWith("System"));

		try
		{
			var compilation = compilationEngine.CreateCompilation(compilationName, code, typesReferences);

			var startTime = DateTime.UtcNow;
			var task = Task.Run(() => executor.Execute(compilation));
			if (!task.Wait(TimeSpan.FromSeconds(1)))
			{
				return new SubmissionModel()
				{
					SBM_Status = ExecutionStatus.Timeout,
					SBM_SubmissionStartTimeUtc = startTime,
					SBM_SubmissionDuration = DateTime.UtcNow - startTime,
					SBM_Code = code,
					SBM_Question = question,
					SBM_SubmittedBy = submitter,
				};
			}
			var testResults = task.Result;
			var testsDuration = DateTime.UtcNow - startTime;

			var allSuccess = !testResults.Any(x => x.TCR_Status != TestCaseStatus.Success);
			var status = allSuccess ? ExecutionStatus.Success : ExecutionStatus.Failure;

			var submissionResult = new SubmissionModel
			{
				SBM_Status = status,
				SBM_SubmissionStartTimeUtc = startTime,
				SBM_SubmissionDuration = testsDuration,
				SBM_TestRuns = testResults,
				SBM_Code = code,
				SBM_SubmittedBy = submitter,
				SBM_Question = question,
			};
			return submissionResult;
		}
		catch (CompilationException compilationException)
		{
			return new SubmissionModel()
			{
				SBM_Status = ExecutionStatus.CompileError,
				SBM_SubmissionStartTimeUtc = DateTime.UtcNow,
				SBM_SubmissionDuration = TimeSpan.Zero,
				SBM_CompileError = compilationException.Message,
				SBM_Code = code,
				SBM_Question = question,
				SBM_SubmittedBy = submitter,
			};
		}
	}
}