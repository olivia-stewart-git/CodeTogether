using System.ComponentModel.DataAnnotations.Schema;
using CodeTogether.Data.DataAccess;
using CodeTogether.Data.Models.Questions;
using Microsoft.EntityFrameworkCore;

namespace CodeTogether.Runner.Engine;

[PrimaryKey(nameof(TRX_PK))]
public class TestRunExecutionModel : IDbModel
{
	public Guid TRX_PK { get; set; } = Guid.NewGuid();

	[InverseProperty(nameof(TestRunModel.TCT_Execution))]
	public required IEnumerable<TestRunModel> TRX_TestRuns { get; set; }
}