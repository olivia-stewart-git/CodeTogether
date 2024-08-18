using CodeTogether.Data.DataAccess;
using CodeTogether.Data.Models.Game;
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

	public bool GM_Private { get; set; }

	public int GM_MaxPlayers { get; set; } = 2;

	public bool GM_WaitForAll { get; set; } = false;

	public Guid? GM_USR_FKWinner { get; set; }

	[MaxLength(int.MaxValue)]
	public string? GM_WinnerCode { get; set; }

	[ForeignKey(nameof(GM_USR_FKWinner))]
	public UserModel? GM_Winner { get; set; }


	[NotMapped]
	public DateTime LastActionTime => GM_StartedAtUtc.HasValue ? GM_StartedAtUtc.Value : GM_CreateTimeUtc;

	public IEnumerable<GamePlayerModel> GamePlayers { get; set; } = new List<GamePlayerModel>();
}
