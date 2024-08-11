using CodeTogether.Data.DataAccess;
using CodeTogether.Data.Models.Game;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CodeTogether.Data.Models.Questions;

[PrimaryKey(nameof(GM_PK))]
public class GameModel : IDbModel
{
	public Guid GM_PK { get; set; } = Guid.NewGuid();

	[MaxLength(100)]
	public required string GM_Name { get; set; }

	public GameState GM_GameState { get; set; } = GameState.Lobby;

	public DateTime GM_CreateTimeUtc { get; set; } = DateTime.UtcNow;

	public DateTime? GM_StartedAt { get; set; }

	public int MaxPlayers { get; set; } = 2;

	public IEnumerable<UserModel> Users { get; set; } = new List<UserModel>();
}

public enum GameState
{
	Lobby, Playing
}
