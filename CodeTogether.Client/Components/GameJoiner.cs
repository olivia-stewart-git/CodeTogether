using CodeTogether.Client.Integration;
using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace CodeTogether.Client.Components
{
	public class GameJoiner
	{
		public static async Task<string> JoinAndNavigateToGame(HttpClient http, NavigationManager navigation, Guid gameId)
		{
			// todo create ApiRequestMaker or something so the paths are in one place, or maybe use swagger
			var response = await http.GetAsync($"/api/game/join?gameId={gameId}");
			if (response.IsSuccessStatusCode)
			{
				var responseString = await response.Content.ReadAsStringAsync();
				var gameDetails = JsonSerializer.Deserialize<JoinGameResponse>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
				navigation.NavigateTo($"lobby/{gameDetails.ServerId}/{gameId}");
				return "";
			}
			else
			{
				return await response.Content.ReadAsStringAsync();
			}
		}
	}
}
