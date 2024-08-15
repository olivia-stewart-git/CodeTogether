using CodeTogether.Data.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeTogether.Data.Models.Questions;

[PrimaryKey(nameof(TST_PK))]
public class TestCaseModel : IDbModel
{
	public Guid TST_PK { get; set; } = Guid.NewGuid();

	public bool TST_IsHidden { get; set; }

	[MaxLength(30)]
	public required string TST_Title { get; set; }

	[MaxLength(100)]
	public required string[] TST_Arguments { get; set; }

	[MaxLength(100)]
	public required string TST_ExpectedResponse { get; set; }

	[DeleteBehavior(DeleteBehavior.NoAction)]
	public required QuestionModel TST_Question { get; set; };
}