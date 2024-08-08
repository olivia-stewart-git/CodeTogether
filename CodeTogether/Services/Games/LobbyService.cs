using CodeTogether.Client.Integration;
using CodeTogether.Data;
using CodeTogether.Data.Models.Questions;

namespace CodeTogether.Service.Games
{
	public class LobbyService(ApplicationDbContext dbContext) : ILobbyService
	{
		public Guid CreateLobby(string lobbyName)
		{
			var game = new GameModel() { GM_Name = lobbyName };
			dbContext.Games.Add(game);
			dbContext.SaveChanges();
			return game.GM_PK;
		}

		public IEnumerable<GameListGameDTO> GetLobbies()
		{
			var lobbies = dbContext.Games.Select(m => new GameListGameDTO { CreatedAt = m.GM_CreateTimeUtc, Name = m.GM_Name, Id = m.GM_PK, NumPlayers = m.Users.Count() });
			return lobbies;
		}

		public void JoinLobby(Guid gameId, Guid userId)
		{
			var user = dbContext.Users.Find(userId) ?? throw new ArgumentException("Invalid user");
			var game = dbContext.Games.Find(gameId) ?? throw new ArgumentException("Invalid game");
			user.USR_Game = game;
			dbContext.SaveChanges();
		}
	}
}
