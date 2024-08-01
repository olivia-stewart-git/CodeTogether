namespace CodeTogether.Runner.Engine;

[Serializable()]
public class ExecutionRuntimeException : Exception
{
	public ExecutionRuntimeException()
	{
	}

	public ExecutionRuntimeException(string message)
		: base(message)
	{
	}

	public ExecutionRuntimeException(string message, Exception inner)
		: base(message, inner)
	{
	}
}