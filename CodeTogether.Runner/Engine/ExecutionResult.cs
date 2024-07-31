using CodeTogether.Data.Models.Questions;

namespace CodeTogether.Runner.Engine;

public class ExecutionResult
{
	public ExecutionResult(ExecutionStatus status, IEnumerable<TestRunModel> testRuns)
	{
		Status = status;
		TestRuns = testRuns;
	}

	public ExecutionStatus Status { get; set; }
	public IEnumerable<TestRunModel> TestRuns { get; set; }
}