using Basic.Reference.Assemblies;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeTogether.Runner.Engine;

public class CompilationEngine : ICompilationEngine
{
	const string ExcludeAssemblyStringsPattern = @"\.(Net|IO|Win32|Data|Diagnostics|Reflection|RunTime|Security|Web|AppContext)";
	public Assembly CreateCompilation(string assemblyName, string sourceCode, IEnumerable<Type>? referenceTypes = null)
	{
		var allReferences = ((IEnumerable<MetadataReference>)Net80.References.All).ToHashSet();
		foreach (var referenceType in (referenceTypes ?? Array.Empty<Type>()).ToHashSet() )
		{
			allReferences.AddAssembly(referenceType);
		}

		// This will prevent some malicous stuff being used, however things like System.IO.File.ReadAll are actually in the System.Runtime.dll assembly not the io one
		// so there is no way to prevent calling it.
		var references = allReferences.Where(r => !Regex.IsMatch(r.Display ?? "", ExcludeAssemblyStringsPattern));

		var tree = SyntaxFactory.ParseSyntaxTree(sourceCode.Trim());
		var compilation = CSharpCompilation.Create(assemblyName + ".dll")
			.WithOptions(
				new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,
					optimizationLevel: OptimizationLevel.Release)
				{  })
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
		// new context prevents using the assemblies that the main application has loaded
		var loadContext = new AssemblyLoadContext("userLoadContext", true);
		codeStream.Position = 0;
		return loadContext.LoadFromStream(codeStream);
	}
}