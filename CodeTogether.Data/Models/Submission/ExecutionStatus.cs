namespace CodeTogether.Runner.Engine;

public enum ExecutionStatus
{
	InProgress,
	Success, // All tests passed
	Failure, // Some tests failed
	Timeout, // Test runs took too long
	CompileError // Failed to build
}