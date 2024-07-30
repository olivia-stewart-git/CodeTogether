using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeTogether.Runner.Scaffolds;

namespace CodeTogether.Runner.Test.Scaffolds;

public class ScaffoldLoaderTest
{
	[Test]
	public void TestLoadsScaffoldFromKey()
	{
		var loader = new ScaffoldLoader();
		var loadedScaffold = loader.LoadScaffold("HelloWorldScaffold");

		Assert.IsNotNull(loadedScaffold);
		Assert.That(loadedScaffold.Contains("public string HelloWorld()"));
	}

	[Test]
	public void TestThrowsOnInvalidScaffold()
	{
		var loader = new ScaffoldLoader();
		Assert.Throws<InvalidOperationException>(() => loader.LoadScaffold("blah blah blah"));
	}
}