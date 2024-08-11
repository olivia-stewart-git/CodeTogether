using CodeTogether.Data.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CodeTogether.Data.Models;

[PrimaryKey(nameof(STM_PK))]
public class StmDataModel : IDbModel
{
	public class Constants
	{
		public const string HasSeeded = "HasSeeded";
		public const string SchemaVersion = "SchemaVersion";
	}

	public Guid STM_PK { get; set; } = Guid.NewGuid();

	[MaxLength(20)]
	public required string STM_Key { get; set; }

	[MaxLength(100)]
	public required string STM_Value { get; set; }
}