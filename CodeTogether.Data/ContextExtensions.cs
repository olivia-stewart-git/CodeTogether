using System.Linq.Expressions;
using System.Text.Json;
using CodeTogether.Data.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace CodeTogether.Data;

public static class ContextExtensions
{
	static readonly JsonSerializerOptions ConversionOptions = new JsonSerializerOptions()
	{
		WriteIndented = true
	};

	public static ModelBuilder HasJsonConversion<T, TProperty>(this ModelBuilder modelBuilder, Expression<Func<T, TProperty?>> setup) where T : class, IDbModel
	{
		modelBuilder.Entity<T>()
			.Property(setup)
			.HasConversion(
				v => JsonSerializer.Serialize(v, ConversionOptions),
				v => JsonSerializer.Deserialize<TProperty>(v, ConversionOptions));
		return modelBuilder;
	}
}