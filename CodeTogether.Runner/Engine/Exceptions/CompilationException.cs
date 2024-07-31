namespace CodeTogether.Runner.Engine;

public class CompilationException : Exception
{
	public CompilationException()
	{
	}

	public CompilationException(string message)
		: base(message)
	{
	}

	public CompilationException(string message, Exception inner)
		: base(message, inner)
	{
	}
}