using Newtonsoft.Json.Linq;
using System.Reflection;

namespace CodeTogether.Runner.Scaffolds;

public class ScaffoldLoader : IScaffoldLoader
{
	public ScaffoldLoader()
	{
		CreateCache();
	}

	readonly Dictionary<string, Scaffold> resourceCache = [];

	void CreateCache()
	{
		resourceCache.Add("HelloWorldScaffold", new Scaffold(LoadScaffoldCode("CodeTogether.Runner.Scaffolds.ScaffoldFiles.HelloWorldScaffold.txt"), []));
		resourceCache.Add("SimpleAddScaffold", new Scaffold(LoadScaffoldCode("CodeTogether.Runner.Scaffolds.ScaffoldFiles.SimpleAddScaffold.txt"), []));
	}

	string LoadScaffoldCode(string name)
	{
		var assembly = Assembly.GetExecutingAssembly();
		using var stream = assembly.GetManifestResourceStream(name);
		if (stream == null)
		{
			throw new FileNotFoundException("Resource not found.", name);
		}

		using StreamReader reader = new StreamReader(stream);
		return reader.ReadToEnd();
	}

	public List<Scaffold> LoadAllScaffolds()
	{
		return resourceCache.Keys.Select(LoadScaffold).ToList();
	}

	public Scaffold LoadScaffold(string scaffoldName)
	{
		if (resourceCache.TryGetValue(scaffoldName, out var value))
		{
			return value;
		}

		throw new InvalidOperationException($"Cannot find scaffold with key {scaffoldName}");
	}
}