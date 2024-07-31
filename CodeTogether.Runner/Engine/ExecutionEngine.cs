namespace CodeTogether.Runner.Engine;

public class ExecutionEngine : IExecutionEngine
{
	readonly ICompilationEngine compilationEngine;

	public ExecutionEngine(ICompilationEngine compilationEngine)
	{
		this.compilationEngine = compilationEngine;
	}
}