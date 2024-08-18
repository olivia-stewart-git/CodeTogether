using CodeTogether.Data.Models.Questions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CodeTogether.Data.DataAccess;
using CodeTogether.Runner.Engine;
using CodeTogether.Data.Models.Game;

namespace CodeTogether.Data.Models.Submission;

[PrimaryKey(nameof(SBM_PK))]
public class SubmissionModel : IDbModel
{
	public Guid SBM_PK { get; set; } = Guid.NewGuid();

	public required DateTime SBM_SubmissionStartTimeUtc { get; set; } = DateTime.UtcNow;

	// Duration that the tests took to run
	public required TimeSpan SBM_SubmissionDuration { get; set; }

	[MaxLength(int.MaxValue)]
	public required string SBM_Code { get; set; } = string.Empty;

	public required QuestionModel SBM_Question { get; set; } = null!;

	public required GamePlayerModel SBM_SubmittedBy { get; set; }

	public required ExecutionStatus SBM_Status { get; set; } = ExecutionStatus.InProgress;

	public string? SBM_CompileError { get; set; }

	[InverseProperty(nameof(TestRunModel.TCR_SubmissionResult))]
	public IEnumerable<TestRunModel> SBM_TestRuns { get; set; } = new List<TestRunModel>();
}