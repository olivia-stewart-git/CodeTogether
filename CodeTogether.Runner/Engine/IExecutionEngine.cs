using CodeTogether.Data.Models.Questions;

namespace CodeTogether.Runner.Engine;

public interface IExecutionEngine
{
	ExecutionResultModel ExecuteAgainstQuestion(QuestionModel question, string code);
}