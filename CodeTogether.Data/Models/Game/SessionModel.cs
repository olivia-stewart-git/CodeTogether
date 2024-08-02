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
	public string? GM_Name { get; set; }

	IEnumerable<UserModel> Users { get; set; } = new List<UserModel>();
}
