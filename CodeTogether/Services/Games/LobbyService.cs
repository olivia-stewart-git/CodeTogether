using CodeTogether.Data;
using CodeTogether.Data.Models.Questions;
using CodeTogether.Service.Games.DTOs;

namespace CodeTogether.Service.Games
{
	public class LobbyService(ApplicationDbContext dbContext) : ILobbyService
	{
		public Guid CreateGame(string lobbyName)
		{
			var game = new GameModel() { GM_Name = lobbyName };
			dbContext.Games.Add(game);
			dbContext.SaveChanges();
			return game.GM_PK;
		}

		public IEnumerable<GameListGameDTO> GetGames()
		{
			dbContext.Games.Select(m => new GameListGameDTO { CreatedAt = m.GM_CreateTime, Name = m.GM_Name, Id = m.GM_PK, NumPlayers = m.Users.Count() });
			return new List<GameListGameDTO>();
		}

		public void JoinGame(Guid gameId, Guid userId)
		{
			var user = dbContext.Users.Find(userId) ?? throw new ArgumentException("Invalid user");
			var game = dbContext.Games.Find(gameId) ?? throw new ArgumentException("Invalid game");
			user.UR_Game = game;
			dbContext.SaveChanges();
		}
	}
}
