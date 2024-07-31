using System.Reflection;

namespace CodeTogether.Runner.Engine;

public class CompilationEngine : ICompilationEngine
{
	public Assembly? CreateCompilation(out IEnumerable<CompilationException> compilationExceptions)
	{
		compilationExceptions = [];
		return null;
	}
}