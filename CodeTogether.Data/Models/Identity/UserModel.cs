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

	public DateTime USR_LastHeardFromAt { get; set; }

	[ForeignKey(nameof(USR_GM_FK))]
	public GameModel? USR_Game { get; set; }
	public Guid? USR_GM_FK { get; set; }
}