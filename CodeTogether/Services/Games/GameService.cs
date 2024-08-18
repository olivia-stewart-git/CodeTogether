using CodeTogether.Data;
using CodeTogether.Data.Models.Questions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace CodeTogether.Services.Games
{
	public class GameService : IGameService
	{
		ConcurrentDictionary<string, GameModel> userToGameCache = new ConcurrentDictionary<string, GameModel>();

		// have to take dbContext as a parameter rather than DI'ing it because this service needs to be scoped to allow caching between signalr requests and ApplicationDbContext is scoped
		public GameModel GetActiveGameForUser(ApplicationDbContext dbContext, string? userIdString, bool cache = true)
		{
			if (userIdString == null)
			{
				throw new ArgumentNullException(nameof(userIdString));
			}

			userIdString = userIdString.ToLower();

			if (cache && userToGameCache.ContainsKey(userIdString))
			{
				return userToGameCache[userIdString];
			}

			var userId = Guid.Parse(userIdString ?? "");

			var user = dbContext.Users.First(x => x.USR_PK == userId);
			var player = dbContext.GamePlayers.Include(p => p.GMP_Game).Where(p => p.GMP_PK == user.USR_GMP_FK).FirstOrDefault();
			var game = player?.GMP_Game;
			if (game == null)
			{
				throw new ArgumentNullException("No active game");
			}
			userToGameCache[userIdString] = game;
			return game;
		}

		void RemoveFromCacche(Guid userId) => userToGameCache.Remove(userId.ToString().ToLower(), out _);

		public void MarkAsFinished(ApplicationDbContext dbContext, Guid gameId)
		{
			// Work around for not being able to broadcast a signalr message from controller (this is apparently possible but wasn't working)
			// so instead just make it so the next key update for that game will realise the game is over and broadcast it from there.
			var userIds = dbContext.GamePlayers.Where(p => p.GMP_GM_FK == gameId).Select(p => p.GMP_USR_FK);
			foreach (var userId in userIds)
			{
				RemoveFromCacche(userId);
			}
			dbContext.Games.First(g => g.GM_PK == gameId).GM_FinishedAtUtc = DateTime.UtcNow;
			dbContext.SaveChanges();
		}
	}
}
