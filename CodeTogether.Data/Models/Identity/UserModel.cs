using CodeTogether.Data.DataAccess;
using CodeTogether.Data.Models.Questions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeTogether.Data.Models.Game;

[PrimaryKey(nameof(USR_PK))]
public class UserModel : IDbModel
{
	public Guid USR_PK { get; set; } = Guid.NewGuid();

	[MaxLength(100)]
	public string USR_Email { get; set; } = string.Empty;

	[MaxLength(100)]
	public string USR_UserName { get; set; } = string.Empty;

	[MaxLength(150)]
	public string USR_PasswordHash { get; set; } = string.Empty;

	[MaxLength(150)]
	public string USR_PasswordSalt { get; set; } = string.Empty;

	public string[] USR_CheckPoints { get; set; } = [];

	public DateTime USR_LastHeardFromAt { get; set; }

	public IEnumerable<GamePlayerModel> GamePlayers { get; set; } = new List<GamePlayerModel>();

	public IEnumerable<GameModel> Games { get; set; } = new List<GameModel>();
}