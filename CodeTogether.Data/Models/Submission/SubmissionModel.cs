using CodeTogether.Data.Models.Questions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeTogether.Data.Models.Submission;

[PrimaryKey(nameof(SBM_PK))]
public class SubmissionModel
{
	public Guid SBM_PK { get; set; } = Guid.NewGuid();

	public DateTime SBM_SubmissionTime { get; set; } = DateTime.Now;

	[MaxLength(int.MaxValue)]
	public required string SBM_Code { get; set; } = string.Empty;

	[ForeignKey(nameof(SBM_QST_FK))]
	public required QuestionModel SBM_Question { get; set; }
	public Guid SBM_QST_FK { get; set; }
}