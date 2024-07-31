using System.Globalization;
using System.Text.Json;

namespace CodeTogether.Runner.Adaptors;

public static class TypeConverter
{
	public static object Convert(string input, Type type)
	{
		switch (Type.GetTypeCode(type))
		{
			case TypeCode.Boolean:
				return bool.Parse(input);
			case TypeCode.Byte:
				return byte.Parse(input);
			case TypeCode.Char:
				return char.Parse(input);
			case TypeCode.DateTime:
				return DateTime.Parse(input, CultureInfo.InvariantCulture);
			case TypeCode.Decimal:
				return decimal.Parse(input, CultureInfo.InvariantCulture);
			case TypeCode.Double:
				return double.Parse(input, CultureInfo.InvariantCulture);
			case TypeCode.Int16:
				return short.Parse(input, CultureInfo.InvariantCulture);
			case TypeCode.Int32:
				return int.Parse(input, CultureInfo.InvariantCulture);
			case TypeCode.Int64:
				return long.Parse(input, CultureInfo.InvariantCulture);
			case TypeCode.SByte:
				return sbyte.Parse(input, CultureInfo.InvariantCulture);
			case TypeCode.Single:
				return float.Parse(input, CultureInfo.InvariantCulture);
			case TypeCode.String:
				return input;
			case TypeCode.UInt16:
				return ushort.Parse(input, CultureInfo.InvariantCulture);
			case TypeCode.UInt32:
				return uint.Parse(input, CultureInfo.InvariantCulture);
			case TypeCode.UInt64:
				return ulong.Parse(input, CultureInfo.InvariantCulture);
			default:
				// Attempt to deserialize JSON for complex types
				try
				{
					return JsonSerializer.Deserialize(input, type)!;
				}
				catch (JsonException)
				{
					throw new InvalidCastException($"Cannot convert '{input}' to type {type}");
				}
		}
	}
}