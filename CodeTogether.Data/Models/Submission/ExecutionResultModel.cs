using System.ComponentModel.DataAnnotations.Schema;
using CodeTogether.Data.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace CodeTogether.Runner.Engine;

[PrimaryKey(nameof(EXR_PK))]
public class ExecutionResultModel : IDbModel
{
	public Guid EXR_PK { get; set; } = Guid.NewGuid();

	public required ExecutionStatus EXR_Status { get; set; } = ExecutionStatus.None;

	[ForeignKey(nameof(EXR_TRX_FK))]
	public required TestRunExecutionModel EXR_TestRun { get; set; }
	public Guid EXR_TRX_FK { get; set; }
}