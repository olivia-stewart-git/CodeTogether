using System.Reflection;

namespace CodeTogether.Runner.Scaffolds;

public class ScaffoldLoader : IScaffoldLoader
{
	public ScaffoldLoader()
	{
		CreateCache();
	}

	readonly Dictionary<string, string> resourceCache = new ();

	void CreateCache()
	{
		var assembly = Assembly.GetExecutingAssembly();
		var resourceNames = assembly.GetManifestResourceNames();
		foreach (var resourceName in resourceNames.Where(x => x.Split('.', StringSplitOptions.RemoveEmptyEntries).Contains("Scaffolds")))
		{
			var withoutExtension = Path.GetFileNameWithoutExtension(resourceName);
			resourceCache.Add(withoutExtension.Split('.', StringSplitOptions.RemoveEmptyEntries).Last(), resourceName);
		}
    }

	public List<string> LoadAllScaffolds()
	{
		return resourceCache.Keys.Select(LoadScaffold).ToList();
	}

	public string LoadScaffold(string scaffoldName)
	{
		var assembly = Assembly.GetExecutingAssembly();
		if (resourceCache.TryGetValue(scaffoldName, out var value))
		{
			using var stream = assembly.GetManifestResourceStream(value);
			if (stream == null)
			{
				throw new FileNotFoundException("Resource not found.", value);
			}

			using StreamReader reader = new StreamReader(stream);
			return reader.ReadToEnd();
		}
		else
		{
			throw new InvalidOperationException($"Cannot find scaffold with key {scaffoldName}");
		}
    }
}