using CodeTogether.Client.Integration;
using CodeTogether.Data;
using CodeTogether.Data.Models.Questions;
using CodeTogether.Service.Games;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CodeTogether.Hubs
{
	public class LobbyHub(ApplicationDbContext dbContext, ILobbyService lobbyService) : Hub
	{
		public override async Task OnConnectedAsync()
		{
			var userId = Guid.Parse(Context.UserIdentifier ?? "");
			var game = dbContext.Games.Include(g => g.Users).Where(g => g.Users.Any(u => u.USR_PK == userId)).First();

			await Groups.AddToGroupAsync(Context.ConnectionId, game.GM_PK.ToString());

			await BroadcastLobbyStateTogroup(game);
		}

		// Used for things like configuration changes and triggering the start of the game
		public async Task UpdateState(SetLobbyConfigurationDTO newState)
		{
			var userId = Guid.Parse(Context.UserIdentifier ?? ""); // TODO: handle expcetion
			var game = dbContext.Games.Include(g => g.Users).Where(g => g.Users.Any(u => u.USR_PK == userId)).First();

			lobbyService.UpdateConfiguration(newState, game);

			await BroadcastLobbyStateTogroup(game);
		}

		async Task BroadcastLobbyStateTogroup(GameModel game)
		{
			var config = new LobbyConfigurationDTO { MaxPlayers = game.MaxPlayers, StartingAtUtc = game.GM_StartedAt };
			var state = new LobbyStateDTO { Configuration = config, Players = game.Users.Select(x => x.USR_UserName) };

			await Clients.Group(game.GM_PK.ToString()).SendAsync("StateHasBeenUpdated", state);
		}
	}
}
