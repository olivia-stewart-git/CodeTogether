namespace CodeTogether.Runner.Engine;

[Serializable()]
public class ExecutionSetupException : Exception
{
	public ExecutionSetupException()
	{
	}

	public ExecutionSetupException(string message)
		: base(message)
	{
	}

	public ExecutionSetupException(string message, Exception inner)
		: base(message, inner)
	{
	}
}