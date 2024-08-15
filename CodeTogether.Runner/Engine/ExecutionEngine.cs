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
		
		var executor = executorFactory.GetExecutor(configuration, question.QST_TestCases)
			?? throw new ExecutionSetupException($"Could not resolve an executor for {configuration.EXE_ExecutionRunnerName}");

		var parameterTypes = question.QST_Scaffold.EXE_Parameters.Select(p => p.TC_Type.OT_Type ?? throw new InvalidOperationException("Scaffold should not have invalid parameter types"));
		var returnType = question.QST_Scaffold.EXE_ReturnType.OT_Type ?? throw new InvalidOperationException("Scaffold should not have invalid return type");
		var typesReferences = parameterTypes.Append(returnType);

		try
		{
			var compilation = compilationEngine.CreateCompilation(compilationName, code, typesReferences);
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