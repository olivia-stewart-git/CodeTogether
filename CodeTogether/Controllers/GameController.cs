using CodeTogether.Service.Games;
using Microsoft.AspNetCore.Mvc;

namespace CodeTogether.Controllers
{
	[Route("api/game")]
	public class GameController(ILobbyService gameService, IUserService userService) : Controller
	{
		[Route("create")]
		public IActionResult CreateGame(string lobbyName)
		{
			var gameId = gameService.CreateGame(lobbyName);
			return Json(gameId);
		}

		[Route("join")]
		public IActionResult JoinGame(string username, Guid gameId)
		{
			// For proper user accounts this should be split into two api calls, one to create a user, one to join a game
			var user = userService.CreateUser(username);
			gameService.JoinGame(gameId, user.UR_PK);
			return Ok();
		}

		[Route("list")]
		public IActionResult ListGames()
		{
			return Json(gameService.GetGames());
		}

		[Route("test")]
		public IActionResult Test()
		{
			return Content("yup, all good!");
		}
	}
}