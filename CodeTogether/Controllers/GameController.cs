using CodeTogether.Client.Integration;
using CodeTogether.Service.Games;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeTogether.Controllers
{
	[Route("api/game")]
	public class GameController(ILobbyService lobbyService) : Controller
	{
		/// <summary>
		/// Returns the game id
		/// </summary>
		[Route("create")]
		public IActionResult CreateGame(string lobbyName)
		{
			if (string.IsNullOrEmpty(lobbyName))
			{
				return BadRequest();
			}
			var gameId = lobbyService.CreateLobby(lobbyName);
			return Content(gameId.ToString());
		}

		/// <summary>
		/// Returns the server id and player id
		/// </summary>
		[Route("join")]
		public IActionResult JoinGame(Guid gameId)
		{
			var userIdString = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
			var userId = Guid.Parse(userIdString);
			try
			{
				lobbyService.JoinLobby(gameId, userId);
			} catch (InvalidOperationException ex)
			{
				return BadRequest(ex.Message);
			}
			return Json(new JoinGameResponse() { ServerId = 1});
		}

		[Route("list")]
		public IActionResult ListGames()
		{
			return Json(lobbyService.GetLobbies());
		}
	}
}
