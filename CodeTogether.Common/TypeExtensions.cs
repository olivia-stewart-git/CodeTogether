using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTogether.Common
{
	public static class TypeExtensions
	{
		public static string GetAliasedName(this Type type)
		{
			return aliases.ContainsKey(type) ? aliases[type] : type.Name;
		}

		// https://stackoverflow.com/a/36406695
		static readonly Dictionary<Type, string> aliases = new Dictionary<Type, string>()
	{
		{ typeof(byte), "byte" },
		{ typeof(sbyte), "sbyte" },
		{ typeof(short), "short" },
		{ typeof(ushort), "ushort" },
		{ typeof(int), "int" },
		{ typeof(uint), "uint" },
		{ typeof(long), "long" },
		{ typeof(ulong), "ulong" },
		{ typeof(float), "float" },
		{ typeof(double), "double" },
		{ typeof(decimal), "decimal" },
		{ typeof(object), "object" },
		{ typeof(bool), "bool" },
		{ typeof(char), "char" },
		{ typeof(string), "string" },
		{ typeof(void), "void" },
		{ typeof(Nullable<byte>), "byte?" },
		{ typeof(Nullable<sbyte>), "sbyte?" },
		{ typeof(Nullable<short>), "short?" },
		{ typeof(Nullable<ushort>), "ushort?" },
		{ typeof(Nullable<int>), "int?" },
		{ typeof(Nullable<uint>), "uint?" },
		{ typeof(Nullable<long>), "long?" },
		{ typeof(Nullable<ulong>), "ulong?" },
		{ typeof(Nullable<float>), "float?" },
		{ typeof(Nullable<double>), "double?" },
		{ typeof(Nullable<decimal>), "decimal?" },
		{ typeof(Nullable<bool>), "bool?" },
		{ typeof(Nullable<char>), "char?" }
	};
	}
}
