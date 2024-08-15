using CodeTogether.Data.Models.Questions;

namespace CodeTogether.Runner.Engine;

public interface IExecutionEngine
{
	SubmissionResultModel ExecuteAgainstQuestion(QuestionModel question, string code);
}