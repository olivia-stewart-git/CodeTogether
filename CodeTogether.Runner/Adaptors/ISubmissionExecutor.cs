using System.Reflection;
using CodeTogether.Data.Models.Questions;

namespace CodeTogether.Runner.Adaptors;

public interface ISubmissionExecutor
{
	List<TestRunModel> Execute(Assembly targetAssembly);
}