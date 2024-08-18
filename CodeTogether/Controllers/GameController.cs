using CodeTogether.Client.Integration;
using CodeTogether.Service.Games;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeTogether.Controllers
{
	[Route("api/game")]
	[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
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
			try
			{
				lobbyService.JoinLobby(gameId, UserId);
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

		[Route("list-for-user")]
		public IActionResult ListGameHistory()
		{
			return Json(new List<GameHistoryGameDTO>());
		}

		Guid UserId => Guid.Parse(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
	}
}
