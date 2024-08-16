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
			if (TryGetGame(out var game))
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
			if (TryGetGame(out var game))
			{
				lobbyService.UpdateConfiguration(newState, game);

				await BroadcastLobbyStateTogroup(game);
			}
			else
			{
				await Clients.Caller.SendAsync("NotInGame");
			}
		}

		bool TryGetGame([NotNullWhen(returnValue: true)] out GameModel? game)
		{
			game = null;

			var validId = Guid.TryParse(Context.UserIdentifier ?? "", out Guid userId);
			if (!validId)
			{
				return false;
			}

			var maybeGame = dbContext.Games.Include(g => g.Users).Where(g => g.Users.Any(u => u.USR_PK == userId)).FirstOrDefault();
			if (maybeGame == null)
			{
				return false;
			}

			game = maybeGame;
			return true;
		}

		async Task BroadcastLobbyStateTogroup(GameModel game)
		{
			var config = new LobbyConfigurationDTO { MaxPlayers = game.GM_MaxPlayers, StartingAtUtc = game.GM_StartedAtUtc, IsPrivate = game.GM_Private };
			var state = new LobbyStateDTO { Configuration = config, Players = game.Users.Select(x => x.USR_UserName) };

			await Clients.Group(game.GM_PK.ToString()).SendAsync("StateHasBeenUpdated", state);
		}
	}
}
