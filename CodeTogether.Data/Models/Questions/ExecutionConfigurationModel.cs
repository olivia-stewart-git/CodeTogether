using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CodeTogether.Data.Models.Questions;

[PrimaryKey(nameof(EXE_PK))]
public class ExecutionConfigurationModel
{
	public ExecutionConfigurationModel(string scaffoldName, string functionName)
	{
		EXE_ScaffoldName = scaffoldName;
		EXE_FunctionName = functionName;
	}

	public Guid EXE_PK { get; set; } = Guid.NewGuid();

	[MaxLength(50)]
	public string EXE_ScaffoldName { get; set; }

	[MaxLength(50)]
	public string EXE_FunctionName { get; set; }
}