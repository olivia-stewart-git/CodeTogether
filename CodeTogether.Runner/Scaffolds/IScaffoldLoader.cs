using CodeTogether.Data.Models.Questions;

namespace CodeTogether.Runner.Scaffolds;

public interface IScaffoldLoader
{
	string LoadScaffold(QuestionModel question);
}