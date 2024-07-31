using Microsoft.CodeAnalysis;

namespace CodeTogether.Runner.Engine;
public static class CompilationExtensions
{
	public static bool AddAssembly(this ISet<MetadataReference> references, Type type)
	{
		try
		{
			var systemReference = MetadataReference.CreateFromFile(type.Assembly.Location);
			references.Add(systemReference);
		}
		catch
		{
			return false;
		}

		return true;
	}
}