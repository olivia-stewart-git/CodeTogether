using CodeTogether.Client.Integration;
using CodeTogether.Data;
using CodeTogether.Data.Models.Questions;
using CodeTogether.Service.Games;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace CodeTogether.Hubs
{
	public class LobbyHub(ApplicationDbContext dbContext, ILobbyService lobbyService) : Hub
	{
		public override async Task OnConnectedAsync()
		{
			var game = GetActiveGameForPlayer();
			if (game != null)
			{
				await Groups.AddToGroupAsync(Context.ConnectionId, game.GM_PK.ToString());

				await BroadcastLobbyStateTogroup(game);
			}
			else
			{
				await Clients.Caller.SendAsync("NotInGame");
			}
		}

		// Used for things like configuration changes and triggering the start of the game
		public async Task UpdateState(SetLobbyConfigurationDTO newState)
		{
			var game = GetActiveGameForPlayer();
			if (game != null)
			{
				lobbyService.UpdateConfiguration(newState, game);

				await BroadcastLobbyStateTogroup(game);
			}
			else
			{
				await Clients.Caller.SendAsync("NotInGame");
			}
		}

		GameModel? GetActiveGameForPlayer()
		{
			var validId = Guid.TryParse(Context.UserIdentifier ?? "", out Guid userId);
			if (!validId)
			{
				return null;
			}

			var games = dbContext.Games.Include(g => g.GamePlayers);
			var game = dbContext.Games.Where(g => g.GamePlayers.Any(u => u.GMP_USR_FK == userId)).FirstOrDefault();
			return game;
		}

		async Task BroadcastLobbyStateTogroup(GameModel game)
		{
			var config = new LobbyConfigurationDTO { MaxPlayers = game.GM_MaxPlayers, StartingAtUtc = game.GM_StartedAtUtc, IsPrivate = game.GM_Private };
			var state = new LobbyStateDTO { Configuration = config, Players = game.GamePlayers.Select(x => x.GMP_User.USR_UserName) };

			await Clients.Group(game.GM_PK.ToString()).SendAsync("StateHasBeenUpdated", state);
		}
	}
}
