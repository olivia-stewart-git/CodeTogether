namespace CodeTogether.Runner.Engine;

public enum ExecutionStatus
{
	InProgress,
	Success, // All tests passed
	Failure, // Some tests failed
	Error // Failed to build
}