using CodeTogether.Data.DataAccess;
using CodeTogether.Data.Models.Game;
using CodeTogether.Data.Models.Submission;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable InconsistentNaming

namespace CodeTogether.Data.Models.Questions;

[PrimaryKey(nameof(GM_PK))]
public class GameModel : IDbModel
{
	public Guid GM_PK { get; set; } = Guid.NewGuid();

	[MaxLength(100)]
	public required string GM_Name { get; set; }

	public DateTime GM_CreateTimeUtc { get; set; } = DateTime.UtcNow;

	public DateTime? GM_StartedAtUtc { get; set; }

	public DateTime? GM_FinishedAtUtc { get; set; }

	public bool GM_Private { get; set; } = false;

	public int GM_MaxPlayers { get; set; } = 2;

	public bool GM_WaitForAll { get; set; } = false;

	public required string GM_CreatedByName { get; set; }

	[ForeignKey(nameof(GM_GM_NextGame_FK))]
	[DeleteBehavior(DeleteBehavior.NoAction)]
	public GameModel? GM_NextGame { get; set; }
	public Guid? GM_GM_NextGame_FK { get; set; }

	[DeleteBehavior(DeleteBehavior.NoAction)]
	public SubmissionModel? GM_WinningSubmission { get; set; }

	[ForeignKey(nameof(GM_QST_FK))]
	[DeleteBehavior(DeleteBehavior.NoAction)]
	public required QuestionModel GM_Question { get; set; }
	public Guid GM_QST_FK { get; set; }

	[NotMapped]
	public DateTime LastActionTime => GM_StartedAtUtc.HasValue ? GM_StartedAtUtc.Value : GM_CreateTimeUtc;

	public IEnumerable<GamePlayerModel> GamePlayers { get; set; } = new List<GamePlayerModel>();
}
