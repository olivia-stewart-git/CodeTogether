using CodeTogether.Client.Integration;
using CodeTogether.Service.Games;
using Microsoft.AspNetCore.Mvc;

namespace CodeTogether.Controllers
{
	[Route("api/game")]
	public class GameController(ILobbyService lobbyService, IUserService userService) : Controller
	{
		/// <summary>
		/// Returns the game id
		/// </summary>
		[Route("create")]
		public IActionResult CreateGame(string lobbyName)
		{
			var gameId = lobbyService.CreateLobby(lobbyName);
			return Content(gameId.ToString());
		}

		/// <summary>
		/// Returns the server id and player id
		/// </summary>
		[Route("join")]
		public IActionResult JoinGame(string username, Guid gameId)
		{
			// For proper user accounts this should be split into two api calls, one to create a user, one to join a game
			var user = userService.CreateUser(username);
			lobbyService.JoinLobby(gameId, user.UR_PK);
			return Json(new JoinGameResponse() { playerId = user.UR_PK, serverId = 1});
		}

		[Route("list")]
		public IActionResult ListGames()
		{
			return Json(lobbyService.GetLobbies());
		}
	}
}
