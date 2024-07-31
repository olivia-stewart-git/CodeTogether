using System.Reflection;

namespace CodeTogether.Runner.Engine;

public interface ICompilationEngine
{
	Assembly? CreateCompilation(string assemblyName, string sourceCode, params Type[] referenceTypes);
}