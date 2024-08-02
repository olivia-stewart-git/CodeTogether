using CodeTogether.Service.Games;
using Microsoft.AspNetCore.Mvc;

namespace CodeTogether.Controllers
{
	public class GameController(IGameService gameService, IUserService userService) : Controller
	{
		public IActionResult CreateGame(string lobbyName)
		{
			var gameId = gameService.CreateGame(lobbyName);
			return Json(gameId);
		}

		public IActionResult JoinGame(string username, Guid gameId)
		{
			// This will be split into two api calls, one to create a user, one to join a game
			var user = userService.CreateUser(username);
			gameService.JoinGame(gameId, user.UR_PK);
			return Ok();
		}

		public IActionResult ListGames()
		{
			return Json(gameService.GetGames());
		}
	}
}
