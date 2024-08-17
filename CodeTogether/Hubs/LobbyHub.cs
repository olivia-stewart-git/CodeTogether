﻿using CodeTogether.Client.Integration;
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

				await BroadcastLobbyStateTogroup(game.GM_PK);
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

				await BroadcastLobbyStateTogroup(game.GM_PK);
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

			var user = dbContext.Users.First(x => x.USR_PK == userId);
			var player = dbContext.GamePlayers.Include(p => p.GMP_Game).Where(p => p.GMP_PK == user.USR_GMP_FK).FirstOrDefault();
			return player?.GMP_Game;
		}

		async Task BroadcastLobbyStateTogroup(Guid gamePk)
		{
			var game = dbContext.Games.Include(g => g.GamePlayers).ThenInclude(gp => gp.GMP_User).First(g => g.GM_PK == gamePk);
			var config = new LobbyConfigurationDTO { MaxPlayers = game.GM_MaxPlayers, StartingAtUtc = game.GM_StartedAtUtc, IsPrivate = game.GM_Private };
			var state = new LobbyStateDTO { Configuration = config, Players = game.GamePlayers.Where(x => x.GMP_GM_FK == game.GM_PK).Select(gp => gp.GMP_User.USR_UserName), Name = game.GM_Name };

			await Clients.Group(game.GM_PK.ToString()).SendAsync("StateHasBeenUpdated", state);
		}
	}
}
