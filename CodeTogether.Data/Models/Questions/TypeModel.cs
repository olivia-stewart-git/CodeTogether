using CodeTogether.Data.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace CodeTogether.Data.Models.Questions;

[PrimaryKey(nameof(OT_PK))]
public class TypeModel : IDbModel
{
    public static TypeModel FromType(Type type)
    {
	    return new TypeModel()
	    {
		    OT_AssemblyName = type.Assembly.FullName ?? throw new InvalidOperationException("Invalid Type used, some types can't be used e.g. generics and arrays"),
		    OT_TypeName = type.FullName ?? throw new InvalidOperationException("Invalid Type used, some types can't be used e.g. generics and arrays"),
		};
    }

	public Guid OT_PK { get; set; } = Guid.NewGuid();

	[MaxLength(500)]
	public required string OT_AssemblyName { get; set; }

	[MaxLength(1000)]
	public required string OT_TypeName { get; set; }

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

			return Type.GetType(OT_TypeName);
		}
		set
		{
			OT_AssemblyName = value?.Assembly.FullName ?? string.Empty;
			OT_TypeName = value?.FullName ?? string.Empty;
		}
	}
}