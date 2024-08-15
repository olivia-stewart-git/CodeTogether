using CodeTogether.Client.Integration;
using CodeTogether.Client.Pages;
using CodeTogether.Data;
using CodeTogether.Data.Models.Game;
using CodeTogether.Data.Models.Questions;
using Microsoft.EntityFrameworkCore;

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

		List<GameListGameDTO>? lobbiesCache;
		DateTime lobbiesCacheExpireAt;
		static readonly TimeSpan cacheExpiry = TimeSpan.FromSeconds(1);

		public IEnumerable<GameListGameDTO> GetLobbies()
		{
			var now = DateTime.Now;
			if (lobbiesCache == null || now > lobbiesCacheExpireAt)
			{
				lobbiesCache = GetLobbiesFromDatabase();
				lobbiesCacheExpireAt = now + cacheExpiry;
			}
			return lobbiesCache;
		}

		List<GameListGameDTO> GetLobbiesFromDatabase()
		{
			var oldGameCutoff = TimeSpan.FromHours(3);
			var now = DateTime.UtcNow;
			return dbContext.Games
				.ToList()
				.Where(g => !g.GM_Private && (now - g.LastActionTime) < oldGameCutoff)
				.Select(m => new GameListGameDTO
				{
					CreatedAt = m.GM_CreateTimeUtc,
					Name = m.GM_Name,
					Id = m.GM_PK,
					NumPlayers = m.Users.Count(),
					Playing = m.GM_GameState == GameState.Playing
				})
				.ToList();
		}

		public bool JoinLobby(Guid gameId, Guid userId)
		{
			if (dbContext.GamePlayers.Any(gp => gp.GMP_USR_FK == userId && gp.GMP_GM_FK == gameId))
			{
				return false;
			}
			dbContext.GamePlayers.Add(new GamePlayerModel { GMP_USR_FK = userId, GMP_GM_FK = gameId} );
			dbContext.SaveChanges();
			return true;
		}

		public GameModel UpdateConfiguration(SetLobbyConfigurationDTO newState, GameModel game)
		{
			game.GM_MaxPlayers = newState.MaxPlayers ?? game.GM_MaxPlayers;
			game.GM_Private = newState.IsPrivate ?? game.GM_Private;

			if (newState.GoingToStart == true)
			{
				var countdownLength = TimeSpan.FromSeconds(5);
				game.GM_StartedAtUtc = DateTime.UtcNow + countdownLength;
				Task.Run(async () =>
				{
					await Task.Delay(countdownLength);
					// TODO: does the captured game update if another message changes it?
					if (DateTime.UtcNow > game.GM_StartedAtUtc)
					{
						game.GM_GameState = GameState.Playing;
						dbContext.SaveChanges();
					}
				});
			}
			if (newState.GoingToStart == false)
			{
				game.GM_StartedAtUtc = null;
			}

			dbContext.SaveChanges();

			return game;
		}
	}
}
