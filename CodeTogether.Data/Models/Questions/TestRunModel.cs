using CodeTogether.Data.DataAccess;
using CodeTogether.Data.Models.Submission;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CodeTogether.Runner.Engine;

namespace CodeTogether.Data.Models.Questions;

[PrimaryKey(nameof(TCR_PK))]
public class TestRunModel : IDbModel
{
	public Guid TCR_PK { get; set; } = Guid.NewGuid();

	public required TestCaseStatus TCR_Status { get; set; }

	[MaxLength(50)]
	public required string TCR_ActualResult { get; set; }

	[MaxLength(400)] 
	public Exception? TCR_Exception { get; set; } = null;

	[ForeignKey(nameof(TCR_TST_FK))]
	public required TestCaseModel TCR_Parent { get; set; }
	public Guid TCR_TST_FK { get; set; }

	[ForeignKey(nameof(TCR_TST_FK))]
    public TestRunExecutionModel? TCT_Execution { get; set; }
	public Guid? TCR_TRX_FK { get; set; }
}