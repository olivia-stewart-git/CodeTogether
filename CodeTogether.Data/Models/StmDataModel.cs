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
	}

    public StmDataModel(string stmKey, string stmValue)
	{
		STM_Key = stmKey;
		STM_Value = stmValue;
	}

    public Guid STM_PK { get; set; } = Guid.NewGuid();

    [MaxLength(20)]
    public string STM_Key { get; set; }

    [MaxLength(100)]
    public string STM_Value { get; set; }
}