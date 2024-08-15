using System.Reflection;
using CodeTogether.Data.Models.Questions;

namespace CodeTogether.Runner.Adaptors;

public interface IExecutorFactory
{
	ISubmissionExecutor? GetExecutor(ScaffoldModel configuration, IEnumerable<TestCaseModel> testCases);
}