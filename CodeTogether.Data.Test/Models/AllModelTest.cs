using CodeTogether.Data.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CodeTogether.Data.Models.Test;

internal partial class AllModelTest
{
	[Test]
	public void TestAllDbModelsHaveUniquePrefix()
	{
        var modelTypes = typeof(IDbModel).Assembly.GetTypes()
			.Where(t => typeof(IDbModel).IsAssignableFrom(t) && t is { IsClass: true, IsAbstract: false });

		var prefixCollection = new HashSet<string>();

		foreach (var modelType in modelTypes)
		{
			var primaryKeyAttr = modelType.GetCustomAttribute<PrimaryKeyAttribute>();

			if (primaryKeyAttr != null)
			{
				var primaryKeyProperty = modelType.GetProperty(primaryKeyAttr.PropertyNames[0]);
				
				if (primaryKeyProperty != null)
				{
					var prefixRegex = PrefixRegex();
					var prefixMatches = prefixRegex.Match(primaryKeyProperty.Name);
					Assert.True(prefixMatches.Success, $"{modelType} should have valid prefix");

					var prefix = prefixMatches.Groups[0].Value.Trim();
					Assert.True(prefixCollection.Add(prefix), $"Prefix: {prefix} exists on more than one model Current: {modelType}");
                }
			}
		}
    }

	[GeneratedRegex("^[^_]{2,3}(?=_)")]
	private static partial Regex PrefixRegex();
}