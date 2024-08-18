using CodeTogether.Client.Integration;
using CodeTogether.Data;
using CodeTogether.Service.Games;
using CodeTogether.Services.Games;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CodeTogether.Controllers
{
	[Route("api/game")]
	[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
	public class GameController(ILobbyService lobbyService, ApplicationDbContext dbContext, QuestionService questionService) : Controller
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
			var gameId = lobbyService.CreateLobby(lobbyName, UserName);
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

		[Route("info")]
		public IActionResult GetGameFinishedInformation(Guid gameId)
		{
			var games = dbContext.Games
				.Include(g => g.GM_WinningSubmission)
				.ThenInclude(s => s.SBM_SubmittedBy)
				.ThenInclude(p => p.GMP_User);
			var game = games.First(x => x.GM_PK == gameId);
			// TODO: maybe have a GameResultModel?
			var winnerName = game.GM_WinningSubmission.SBM_SubmittedBy.GMP_User.USR_UserName;
			var winnerCode = game.GM_WinningSubmission.SBM_Code;
			var players = game.GamePlayers;
			var question = questionService.GetQuestionById(game.GM_QST_FK) ?? throw new InvalidOperationException("Should have a question when finishing a game");

			var result = new GameWinInfoDTO { GameName = game.GM_Name, WinnerName = winnerName, WinnerCode = winnerCode, Question = question };
			return Json(result);
		}

		Guid UserId => Guid.Parse(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

		string UserName => HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
	}
}
