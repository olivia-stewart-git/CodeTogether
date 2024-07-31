using Basic.Reference.Assemblies;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;
using System.Text;

namespace CodeTogether.Runner.Engine;

public class CompilationEngine : ICompilationEngine
{
	public Assembly CreateCompilation(string assemblyName, string sourceCode, params Type[] referenceTypes)
	{
		var references = new HashSet<MetadataReference>();
		foreach (var referenceType in referenceTypes)
		{
			references.AddAssembly(referenceType);
		}

		var tree = SyntaxFactory.ParseSyntaxTree(sourceCode.Trim());
		var compilation = CSharpCompilation.Create(assemblyName + ".dll")
			.WithOptions(
				new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,
					optimizationLevel: OptimizationLevel.Release))
			.WithReferences(references)
			.WithReferenceAssemblies(ReferenceAssemblyKind.Net80)
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

		return Assembly.Load(codeStream.ToArray()); ;
	}
}