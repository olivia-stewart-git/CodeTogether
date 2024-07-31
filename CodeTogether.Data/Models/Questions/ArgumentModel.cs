using CodeTogether.Data.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace CodeTogether.Data.Models.Questions;

[PrimaryKey(nameof(OT_PK))]
public class ArgumentModel : IDbModel
{
    public static ArgumentModel FromType(Type type)
    {
	    return new ArgumentModel()
	    {
		    OT_AssemblyName = type.Assembly.FullName, 
		    OT_TypeName = type.FullName
	    };

    }

	public Guid OT_PK { get; set; } = Guid.NewGuid();

	[ForeignKey(nameof(OT_TC_FK))]
	[DeleteBehavior(DeleteBehavior.NoAction)]
	public ArgumentCollectionModel? OT_Parent { get; set; }
	public Guid? OT_TC_FK { get; set; }

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

			return Type.GetType(OT_TypeName);
		}
		set
		{
			OT_AssemblyName = value?.Assembly.FullName ?? string.Empty;
			OT_TypeName = value?.FullName ?? string.Empty;
		}
	}
}