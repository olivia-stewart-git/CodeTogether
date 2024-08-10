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
			// TODO: get user from auth
			//lobbyService.JoinLobby(gameId, user.USR_PK);
			//return Json(new JoinGameResponse() { PlayerId = user.USR_PK, ServerId = 1});
			throw new NotImplementedException();
		}

		[Route("list")]
		public IActionResult ListGames()
		{
			return Json(lobbyService.GetLobbies());
		}
	}
}
