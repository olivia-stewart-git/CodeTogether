﻿using CodeTogether.Client.Integration;
using CodeTogether.Data;
using CodeTogether.Services.Games;
using Microsoft.AspNetCore.SignalR;

namespace CodeTogether.Hubs;

// A SignalR 'Hub' to implement live typing and over server push features
public class GameHub(ApplicationDbContext dbContext, IGameService gameService) : Hub
{
	public override async Task OnConnectedAsync()
	{
		var game = gameService.GetActiveGameForUser(dbContext, Context.UserIdentifier, cache: false);
		if (game.GM_FinishedAtUtc.HasValue && game.GM_FinishedAtUtc.Value > DateTime.UtcNow)
		{
			await BroadcastFinishedGame(game.GM_PK, game.GM_GM_NextGame_FK);
		}
		else
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, game.GM_PK.ToString());
		}
	}

	public async Task SendKeyPresses(Guid userId, List<KeyPressDTO> keyPresses)
	{
		var game = gameService.GetActiveGameForUser(dbContext, Context.UserIdentifier, cache: true);
		await Clients.OthersInGroup(game.GM_PK.ToString()).SendAsync("ReceiveKeyPresses", userId, keyPresses);

		if (game.GM_FinishedAtUtc != null && game.GM_FinishedAtUtc < DateTime.UtcNow)
		{
			await BroadcastFinishedGame(game.GM_PK, game.GM_GM_NextGame_FK);
		}
	}

	public async Task BroadcastFinishedGame(Guid gameId, Guid? nextGameId)
	{
		await Clients.Group(gameId.ToString()).SendAsync("GameFinished", nextGameId);
	}
}