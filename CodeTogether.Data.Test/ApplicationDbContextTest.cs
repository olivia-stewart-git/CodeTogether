using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeTogether.Data.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace CodeTogether.Data.Test;

internal class ApplicationDbContextTest
{
	[Test]
	public void TestAllSetsAreDbModel()
	{
		var allDbSetTypes = typeof(ApplicationDbContext)
			.GetProperties()
			.Where(p => p.PropertyType.IsGenericType &&
				p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
			.Select(x => x.PropertyType.GetGenericArguments().First())
			.ToList();

		Assert.Multiple(() =>
		{
			foreach (var modelType in allDbSetTypes)
			{
				Assert.True(typeof(IDbModel).IsAssignableFrom(modelType), $"Model {modelType} should be of type IDbModel");
			}
		});
	}
}