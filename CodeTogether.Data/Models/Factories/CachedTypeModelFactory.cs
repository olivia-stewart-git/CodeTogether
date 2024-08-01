using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeTogether.Data.Models.Questions;

namespace CodeTogether.Data.Models.Factories;

public interface ICachedTypeModelFactory
{
	public TypeModel Get(Type type);
}

public class CachedCachedTypeModelFactory : ICachedTypeModelFactory
{
	ConcurrentDictionary<Type, TypeModel> typeCache = new ();

	public TypeModel Get(Type type)
	{
		if (typeCache.TryGetValue(type, out var model))
		{
			return model;
		}

		model = TypeModel.FromType(type);
		typeCache.TryAdd(type, model);
		return model;
	}
}