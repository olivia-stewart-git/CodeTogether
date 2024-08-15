using CodeTogether.Data.Models.Questions;

namespace CodeTogether.Runner.Adaptors;

public class ExecutorFactory : IExecutorFactory
{
	public ISubmissionExecutor? GetExecutor(ScaffoldModel configuration, IEnumerable<TestCaseModel> testCases)
	{
		switch (configuration.EXE_ExecutionRunnerName)
		{
			case ExecutionRunnerType.ClassInstance:
				return new ClassInstanceSubmissionExecutor(configuration, testCases);
			default:
				return null;
		}
	}
}