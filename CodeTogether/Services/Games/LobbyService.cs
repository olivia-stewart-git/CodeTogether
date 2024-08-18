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
		public Guid CreateLobby(string lobbyName, string creatorName)
		{
			var game = new GameModel() { GM_Name = lobbyName, GM_CreatedByName = creatorName, GM_Question = dbContext.Questions.First()};
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
			return dbContext.Games
				.Include(game => game.GamePlayers)
				.ToList()
				.Where(GameShouldBeShownInGameList)
				.Select(m => new GameListGameDTO
				{
					CreatedAt = m.GM_CreateTimeUtc,
					CreatedBy = m.GM_CreatedByName,
					Name = m.GM_Name,
					Id = m.GM_PK,
					NumPlayers = m.GamePlayers.Count(),
					MaxPlayers = m.GM_MaxPlayers,
					Playing = m.GM_StartedAtUtc != null,
				})
				.ToList();
		}

		static readonly TimeSpan lobbyGameExpiry = TimeSpan.FromHours(3);

		bool GameShouldBeShownInGameList(GameModel g)
		{
			var now = DateTime.UtcNow;
			return
				!g.GM_Private
				&& (now - g.LastActionTime) < lobbyGameExpiry // game was not started and is old
				&& (g.GM_FinishedAtUtc == null || now < g.GM_FinishedAtUtc); // game is finished
		}

		public void JoinLobby(Guid gameId, Guid userId)
		{
			var alreadyInGame = dbContext.GamePlayers.Any(gp => gp.GMP_USR_FK == userId && gp.GMP_GM_FK == gameId);
			if (alreadyInGame)
			{
				return;
			}

			var game = dbContext.Games.Include(g => g.GamePlayers).First(g => g.GM_PK == gameId);
			if (game.GM_StartedAtUtc != null)
			{
				throw new InvalidOperationException("Game already started");
			}

			if (game.GamePlayers.Count() >= game.GM_MaxPlayers)
			{
				throw new InvalidOperationException("Game full");
			}

			var user = dbContext.Users.Where(u => u.USR_PK == userId).First();

			var player = new GamePlayerModel { GMP_USR_FK = userId, GMP_GM_FK = gameId };
			dbContext.GamePlayers.Add(player);
			user.USR_CurrentGame = player;
			dbContext.SaveChanges();
		}

		public GameModel UpdateConfiguration(SetLobbyConfigurationDTO newState, GameModel game)
		{
			game.GM_MaxPlayers = newState.MaxPlayers ?? game.GM_MaxPlayers;
			game.GM_Private = newState.IsPrivate ?? game.GM_Private;

			if (newState.GoingToStart == true)
			{
				var countdownLength = TimeSpan.FromSeconds(3);
				game.GM_StartedAtUtc = DateTime.UtcNow + countdownLength;
				Task.Run(async () =>
				{
					await Task.Delay(countdownLength).ConfigureAwait(true);
					if (DateTime.UtcNow > game.GM_StartedAtUtc)
					{
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
