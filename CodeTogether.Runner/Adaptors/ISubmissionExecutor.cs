using System.Reflection;
using CodeTogether.Data.Models.Questions;
using CodeTogether.Runner.Engine;

namespace CodeTogether.Runner.Adaptors;

public interface ISubmissionExecutor
{
	SubmissionResultModel Execute(Assembly targetAssembly, QuestionModel question);
}