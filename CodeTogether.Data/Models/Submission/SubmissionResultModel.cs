using System.ComponentModel.DataAnnotations.Schema;
using CodeTogether.Data.DataAccess;
using CodeTogether.Data.Models.Questions;
using CodeTogether.Data.Models.Submission;
using Microsoft.EntityFrameworkCore;

namespace CodeTogether.Runner.Engine;

[PrimaryKey(nameof(EXR_PK))]
public class SubmissionResultModel : IDbModel
{
	public Guid EXR_PK { get; set; } = Guid.NewGuid();

	public required ExecutionStatus EXR_Status { get; set; } = ExecutionStatus.InProgress;

	[InverseProperty(nameof(TestRunModel.TCR_SubmissionResult))]
	public IEnumerable<TestRunModel> EXR_TestRuns { get; set; } = new List<TestRunModel>();

	public Exception? EXR_CompileError { get; set; }
}