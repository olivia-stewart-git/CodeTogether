using System.Reflection;
using CodeTogether.Data.Models.Questions;

namespace CodeTogether.Runner.Adaptors;

public interface IExecutorFactory
{
	ISubmissionExecutor? TryGetExecutor(ExecutionConfigurationModel configuration,
		IEnumerable<TestCaseModel> testCases);
}