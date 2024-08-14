using CodeTogether.Data.DataAccess;
using CodeTogether.Data.Models.Game;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeTogether.Data.Models.Questions;

[PrimaryKey(nameof(GM_PK))]
public class GameModel : IDbModel
{
	public Guid GM_PK { get; set; } = Guid.NewGuid();

	[MaxLength(100)]
	public required string GM_Name { get; set; }

	public GameState GM_GameState { get; set; } = GameState.Lobby;

	public DateTime GM_CreateTimeUtc { get; set; } = DateTime.UtcNow;

	public DateTime? GM_StartedAtUtc { get; set; }

	public bool GM_Private { get; set; }

	public int GM_MaxPlayers { get; set; } = 2;

	[NotMapped]
	public DateTime LastActionTime => GM_StartedAtUtc.HasValue ? GM_StartedAtUtc.Value : GM_CreateTimeUtc;

	public IEnumerable<UserModel> Users { get; set; } = new List<UserModel>();
}

public enum GameState
{
	Lobby, Playing
}
