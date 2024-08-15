using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CodeTogether.Data.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace CodeTogether.Data.Models.Questions;

// Holds the scaffold and signature that many questions can use
[PrimaryKey(nameof(EXE_PK))]
public class ScaffoldModel : IDbModel
{
	public Guid EXE_PK { get; set; } = Guid.NewGuid();

	[MaxLength(50)] 
	public required string EXE_ScaffoldName { get; set; }

	[MaxLength(50)]
	public required string EXE_ExecutionRunnerName { get; set; }

	[DeleteBehavior(DeleteBehavior.NoAction)]
	public required TypeModel EXE_ReturnArgument { get; set; }

	[InverseProperty(nameof(ParameterModel.TC_Scaffold))]
	public IEnumerable<ParameterModel> EXE_Parameters { get; set; } = [];

}