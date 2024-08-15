using CodeTogether.Data.Models.Questions;
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

	public SubmissionResultModel ExecuteAgainstQuestion(QuestionModel question, string code)
	{
		var configuration = question.QST_Scaffold;
		var compilationName = $"Compilation_{question.QST_Name}{Guid.NewGuid()}";

		if(!executorFactory.TryGetExecutor(configuration, question.QST_TestCases, out var executor))
		{
			throw new ExecutionSetupException($"Could not resolve an executor for {configuration.EXE_ExecutionRunnerName}");
		}

		try
		{
			var compilation = compilationEngine.CreateCompilation(compilationName, code);
			return executor.Execute(compilation);
		}
		catch (CompilationException compilationException)
		{
			return new SubmissionResultModel()
			{
				EXR_Status = ExecutionStatus.Error,
				EXR_CompileError = compilationException
			};
		}
	}
}