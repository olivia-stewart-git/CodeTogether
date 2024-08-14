using CodeTogether.Data.Models.Questions;

namespace CodeTogether.Runner.Adaptors;

public class ExecutorFactory : IExecutorFactory
{
	public ISubmissionExecutor? TryGetExecutor(
		ExecutionConfigurationModel configuration, 
		IEnumerable<TestCaseModel> testCases)
	{
		switch (configuration.EXE_ExecutionRunnerName)
		{
			case nameof(ClassInstanceSubmissionExecutor):
				return new ClassInstanceSubmissionExecutor(configuration, testCases);
			default:
				return null;
		}

	}
}