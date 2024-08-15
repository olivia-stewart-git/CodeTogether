using System.Reflection;
using CodeTogether.Data.Models.Questions;
using CodeTogether.Data.Models.Submission;
using CodeTogether.Runner.Adaptors;
using CodeTogether.Runner.Scaffolds;

namespace CodeTogether.Runner.Engine;

public class ExecutionEngine : IExecutionEngine
{
	readonly ICompilationEngine compilationEngine;
	readonly IExecutorFactory executorFactory;
	readonly IScaffoldLoader scaffoldLoader;

	public ExecutionEngine(ICompilationEngine compilationEngine, IExecutorFactory executorFactory, IScaffoldLoader scaffoldLoader)
	{
		this.compilationEngine = compilationEngine;
		this.executorFactory = executorFactory;
		this.scaffoldLoader = scaffoldLoader;
	}

	public SubmissionResultModel ExecuteAgainstQuestion(QuestionModel question, string code)
	{
		var configuration = question.QST_Scaffold;
		var compilationName = $"Compilation_{question.QST_Name}{Guid.NewGuid()}";

		var scaffold = scaffoldLoader.LoadScaffold(question);

		if(!executorFactory.TryGetExecutor(configuration, question.QST_TestCases, out var executor))
		{
			throw new ExecutionSetupException($"Could not resolve an executor for {configuration.EXE_ExecutionRunnerName}");
		}

		try
		{
			var compilation = compilationEngine.CreateCompilation(compilationName, code, scaffold.ReferencedTypes);
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