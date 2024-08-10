using CodeTogether.Client.Integration;
using CodeTogether.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CodeTogether.Hubs
{
	public class LobbyHub(ApplicationDbContext dbContext) : Hub
	{
		public async Task RequestState()
		{
			var userId = Context.UserIdentifier;
			var queryable = dbContext.Users.Include(u => u.USR_Game).ThenInclude(g => g.Users);
			var user = queryable.First(x => x.USR_Email == userId);
			var game = user.USR_Game;
			var config = new LobbyConfigurationDTO { MaxPlayers = game.MaxPlayers, StartingAtUtc = null };
			var state = new LobbyStateDTO { Players = game.Users.Select(x => x.USR_UserName), Configuration = config };
			await Clients.Caller.SendAsync("StateHasBeenUpdated", state);
		}

		// Used for things like configuration changes and triggering the start of the game
		public async Task UpdateState(SetLobbyConfigurationDTO newState)
		{
			// get game
			// update gameModel based on newState
			// SaveChanges()
			// boardcast to entire group (including caller)
			// if the state changed to start the game, start a task actually start the game after the countdown
			// if the state changed to no longer be starting the game, cancel the countdown
		}
	}
}
