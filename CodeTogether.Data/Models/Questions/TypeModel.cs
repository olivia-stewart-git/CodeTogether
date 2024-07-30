using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace CodeTogether.Data.Models.Questions;

public class TypeCollectionModel
{
	public Guid
}

[PrimaryKey(nameof(OT_PK))]
public class TypeModel
{
	public static TypeModel Null() => new (null, null);

	public static TypeModel FromType(Type type)
	{
		return new TypeModel(type.Assembly.FullName, type.FullName);
	}

	public TypeModel(string? assemblyName, string? typeName)
	{
		OT_AssemblyName = assemblyName;
		OT_TypeName = typeName;
	}

	public Guid OT_PK { get; set; } = Guid.NewGuid();

	[MaxLength(100)]
	public string? OT_AssemblyName { get; set; }

	[MaxLength(100)]
	public string? OT_TypeName { get; set; }

	[NotMapped]
	public Type? OT_Type
	{
		get
		{
			if (string.IsNullOrEmpty(OT_AssemblyName) || string.IsNullOrEmpty(OT_TypeName))
			{
				return null;
			}

			Assembly? assembly = null;
			try
			{
				assembly = Assembly.Load(OT_AssemblyName);
			}
			catch
			{
			}

			if (assembly != null)
			{
				return assembly.GetType(OT_TypeName);
			}
			else
			{
				return Type.GetType(OT_TypeName);
			}

			return null;
		}
		set
		{
			OT_AssemblyName = value?.Assembly.FullName ?? string.Empty;
			OT_TypeName = value?.FullName ?? string.Empty;
		}
	}
}