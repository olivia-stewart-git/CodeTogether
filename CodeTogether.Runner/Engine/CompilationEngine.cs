using Basic.Reference.Assemblies;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;
using System.Text;

namespace CodeTogether.Runner.Engine;

public class CompilationEngine : ICompilationEngine
{
	public Assembly CreateCompilation(string assemblyName, string sourceCode, IEnumerable<Type>? referenceTypes = null)
	{
		var references = ((IEnumerable<MetadataReference>)Net80.References.All).ToHashSet();
		// TODO: don't reinclude system types?
		foreach (var referenceType in (referenceTypes ?? Array.Empty<Type>()).ToHashSet() )
		{
			references.AddAssembly(referenceType);
		}

		var tree = SyntaxFactory.ParseSyntaxTree(sourceCode.Trim());
		var compilation = CSharpCompilation.Create(assemblyName + ".dll")
			.WithOptions(
				new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,
					optimizationLevel: OptimizationLevel.Release))
			.WithReferences(references)
			.AddSyntaxTrees(tree);

		using var codeStream = new MemoryStream();
		var compilationResult = compilation.Emit(codeStream);

		// Compilation Error handling
		if (!compilationResult.Success)
		{
			var sb = new StringBuilder();
			foreach (var diagnostic in compilationResult.Diagnostics)
			{
				sb.AppendLine(diagnostic.ToString());
			}

			var errorMessage = sb.ToString();

			if (!string.IsNullOrEmpty(errorMessage))
			{
				throw new CompilationException(errorMessage);
			}
		}

		return Assembly.Load(codeStream.ToArray());
	}
}