using CodeTogether.Data.Models.Game;
using CodeTogether.Data.Models.Questions;
using CodeTogether.Data.Models.Submission;

namespace CodeTogether.Runner.Engine;

public interface IExecutionEngine
{
	SubmissionModel ExecuteAgainstQuestion(QuestionModel question, string code, GamePlayerModel submitter);
}