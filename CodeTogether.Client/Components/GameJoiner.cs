using CodeTogether.Client.Integration;
using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace CodeTogether.Client.Components
{
	public class GameJoiner
	{
		public static async Task JoinAndNavigateToGame(HttpClient http, NavigationManager navigation, Guid gameId, string username)
		{
			// todo create ApiRequestMaker or something so the paths are in one place, or maybe use swagger
			var responseString = await http.GetStringAsync($"/api/game/join?gameId={gameId}&username={username}");
			Console.WriteLine(responseString);
			var response = JsonSerializer.Deserialize<JoinGameResponse>(responseString);
			navigation.NavigateTo($"game/{response.serverId}/{response.playerId}");
		}
	}
}
