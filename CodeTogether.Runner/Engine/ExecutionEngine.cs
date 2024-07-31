using CodeTogether.Data.Models.Questions;
using CodeTogether.Data.Models.Submission;

namespace CodeTogether.Runner.Engine;

public class ExecutionEngine : IExecutionEngine
{
	readonly ICompilationEngine compilationEngine;

	public ExecutionEngine(ICompilationEngine compilationEngine)
	{
		this.compilationEngine = compilationEngine;
	}

	public ExecutionResult ExecuteAgainstQuestion(QuestionModel question, string code)
	{
		var configuration = question.QST_ExecutionConfigurationModel;

		return null;
	}
}