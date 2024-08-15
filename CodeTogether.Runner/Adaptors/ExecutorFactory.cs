using CodeTogether.Data.Models.Questions;

namespace CodeTogether.Runner.Adaptors;

public class ExecutorFactory : IExecutorFactory
{
	public bool TryGetExecutor(
		ScaffoldModel configuration, 
		IEnumerable<TestCaseModel> testCases, 
		out ISubmissionExecutor submissionExecutor)
	{
		switch (configuration.EXE_ExecutionRunnerName)
		{
			case nameof(ClassInstanceSubmissionExecutor):
				submissionExecutor = new ClassInstanceSubmissionExecutor(configuration, testCases);
				return true;
		}

		submissionExecutor = null;
		return false;
	}
}