using CodeTogether.Client.Integration;
using CodeTogether.Data;
using CodeTogether.Data.Models.Questions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CodeTogether.Hubs
{
	public class LobbyHub(ApplicationDbContext dbContext) : Hub
	{
		public async Task RequestState(Guid gameId)
		{
			// GameId is used instead of principal userId to remove the extra join
			var game = dbContext.Games.Include(g => g.Users).First(g => g.GM_PK == gameId);
			var config = new LobbyConfigurationDTO { MaxPlayers = game.MaxPlayers, StartingAtUtc = null };
			var state = new LobbyStateDTO { Players = game.Users.Select(x => x.USR_UserName), Configuration = config };

			await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());

			// Sent to whole group so they see the new player
			await Clients.Group(game.GM_PK.ToString()).SendAsync("StateHasBeenUpdated", state);
		}

		// Used for things like configuration changes and triggering the start of the game
		public async Task UpdateState(SetLobbyConfigurationDTO newState)
		{
			// We don't want users to be able to update other peoples games so get the game from the user id rather than allowing them to pass it in
			var userId = Guid.Parse(Context.UserIdentifier ?? ""); // TODO: handle expcetion
			var game = dbContext.Games.Include(g => g.Users).Where(g => g.Users.Any(u => u.USR_PK == userId)).First();

			game.MaxPlayers = newState.MaxPlayers ?? game.MaxPlayers;
			game.GameState = newState.GoingToStart switch
			{
				true => GameState.Starting,
				false => GameState.Lobby,
				null => game.GameState,
			};
			dbContext.SaveChanges();
			var config = new LobbyConfigurationDTO { MaxPlayers = game.MaxPlayers, StartingAtUtc = null };
			var state = new LobbyStateDTO { Configuration = config, Players = game.Users.Select(x => x.USR_UserName) };
			await Clients.Group(game.GM_PK.ToString()).SendAsync("StateHasBeenUpdated", state);
			// if the state changed to start the game, start a task actually start the game after the countdown
			// if the state changed to no longer be starting the game, cancel the countdown
		}
	}
}
