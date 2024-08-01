using System.Reflection;
using CodeTogether.Data.Models.Questions;

namespace CodeTogether.Runner.Adaptors;

public interface IExecutorFactory
{
	bool TryGetExecutor(ExecutionConfigurationModel configuration,
		IEnumerable<TestCaseModel> testCases,
		out ISubmissionExecutor submissionExecutor);
}