using System.Reflection;
using CodeTogether.Runner.Scaffolds;

namespace CodeTogether.Runner.Engine.Test;

internal class CompilationEngineTest
{
	[Test]
	public void TestThrowsCompilationException()
	{
		var compilationEngine = new CompilationEngine();
		Assert.Throws<CompilationException>(() => compilationEngine.CreateCompilation("Compilation", @"
public static vod Main(string[] args) {
	return 0;
}"));
	}

	[Test]
	public void TestScaffoldsCompile()
	{
		var compilationEngine = new CompilationEngine();
		var scaffoldLoader = new ScaffoldLoader();
		Assert.Multiple(() =>
		{
			foreach (var scaffoldCode in scaffoldLoader.LoadAllScaffolds())
			{
				Assert.DoesNotThrow(() => compilationEngine.CreateCompilation("Compilation", scaffoldCode));
			}
		});
	}

	[Test]
	public void TestCanReflectUponCode()
	{
		const string typeName = "MyType";
		var compilationEngine = new CompilationEngine();
		var code = $@"
public class {typeName}
{{
//Do nothing
}}
";

		var assembly = compilationEngine.CreateCompilation("myAssembly", code);

		var types = assembly.GetTypes().Select(x => x.Name).ToHashSet();
		Assert.That(types.Contains(typeName));
	}

	[Test]
	public void TestCanImportTypes()
	{
		const string testCode = @"
using CodeTogether.Runner.Engine.Test;

public class TestClass 
{
	TestInputClass classInstance;

	public TestClass(TestInputClass inputClass) 
	{
		this.classInstance = inputClass;
	}

	public string GetValue() => classInstance.value;
}
";

		var compilationEngine = new CompilationEngine();

		Assembly? assembly = null;
		Assert.DoesNotThrow(() =>
		{
			assembly = compilationEngine.CreateCompilation("myAssembly", testCode, typeof(TestInputClass));
		});
		Assert.IsNotNull(assembly);
		var classType = assembly.GetType("TestClass");
		Assert.IsNotNull(classType);

		var inputObject = new TestInputClass("Passed!");
		var classInstance = Activator.CreateInstance(classType, inputObject);
		Assert.IsNotNull(classInstance);

        var method = classType.GetMethod("GetValue");
		Assert.IsNotNull(method);

		var result = (string)method.Invoke(classInstance, [])!;
		Assert.That(result, Is.EqualTo("Passed!"));
	}
}

public class TestInputClass
{
	public string value;

	public TestInputClass(string value)
	{
		this.value = value;
	}
}