using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CodeTogether.Data.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace CodeTogether.Data.Models.Questions;

[PrimaryKey(nameof(EXE_PK))]
public class ExecutionConfigurationModel : IDbModel
{
	public Guid EXE_PK { get; set; } = Guid.NewGuid();

	[MaxLength(50)]
	public required string EXE_ScaffoldName { get; set; }

	[MaxLength(50)]
	public required string EXE_AdapterName { get; set; }

	[MaxLength(50)]
	public required string EXE_AdapterArgument { get; set; }

	[ForeignKey(nameof(EXE_TC_FK))]
	[DeleteBehavior(DeleteBehavior.NoAction)]
	public ArgumentCollectionModel? EXE_InputArguments { get; set; }
	public Guid? EXE_TC_FK { get; set; }

    [ForeignKey(nameof(EXE_TO_FK))]
	[DeleteBehavior(DeleteBehavior.NoAction)]
	public TypeModel? EXE_ReturnArgument { get; set; }
    public Guid? EXE_TO_FK { get; set; }
}