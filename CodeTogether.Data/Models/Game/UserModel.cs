using CodeTogether.Data.DataAccess;
using CodeTogether.Data.Models.Questions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CodeTogether.Data.Models.Game
{
	[PrimaryKey(nameof(UR_PK))]
	public class UserModel : IDbModel
	{
		public Guid UR_PK { get; set; } = Guid.NewGuid();

		[MaxLength(100)]
		public required string UR_Name { get; set; }

		public DateTime UR_LastHeardFromAt { get; set; }

		public required GameModel UR_Game { get; set; }
	}
}
