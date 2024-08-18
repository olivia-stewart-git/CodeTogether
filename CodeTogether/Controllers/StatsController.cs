using System.Security.Claims;
using CodeTogether.Client.Integration;
using CodeTogether.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeTogether.Controllers;

[Route("api/stats")]
public class StatsController(ApplicationDbContext dbContext) : Controller
{
	[HttpGet("stats")]
	public IActionResult GetStats()
	{
		if (!Guid.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out var userId))
		{
			return Forbid();
		}
		var gamesPlayed = dbContext.Games.Include(g => g.GamePlayers).Count(g => g.GamePlayers.Any(gp => gp.GMP_USR_FK == userId));
		var gamesWithWinners = dbContext.Games.Include(g => g.GM_WinningSubmission).ThenInclude(s => s.SBM_SubmittedBy).ThenInclude(p => p.GMP_User);
		// have to user ternary because lambdas cant have null propagating operator
		var gamesWon = gamesWithWinners.Count(g => g.GM_WinningSubmission == null ? false : g.GM_WinningSubmission.SBM_SubmittedBy.GMP_User.USR_PK == userId);
		return Json(new StatsDTO { GamesPlayed = gamesPlayed, GamesWon = gamesWon });
	}

	[HttpGet("games")]
	public IActionResult GetGames([FromQuery(Name = "pageNum")] int? pageNum, [FromQuery(Name = "pageSize")] int? pageSize)
	{
		if (!Guid.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out var userId))
		{
			return Forbid();
		}

		var actualPageNum = pageNum ?? 1;
		var actualPageSize = pageSize ?? 5;

		var gamesWithWinners = dbContext.Games.Include(g => g.GamePlayers).Include(g => g.GM_WinningSubmission).ThenInclude(s => s.SBM_SubmittedBy).ThenInclude(p => p.GMP_User);
		var gamesPlayed = gamesWithWinners.Where(g => g.GamePlayers.Any(gp => gp.GMP_USR_FK == userId) && g.GM_FinishedAtUtc != null);
		var relevant = gamesPlayed.OrderBy(gp => gp.GM_FinishedAtUtc).Skip((actualPageNum - 1) * actualPageSize).Take(actualPageSize);
		return Ok(relevant.Select(g => new GameResultDTO
		{
			GameName = g.GM_Name,
			GameFinishedUtc = g.GM_FinishedAtUtc!.Value,
			WinnerUsername = g.GM_WinningSubmission.SBM_SubmittedBy.GMP_User.USR_UserName,
			WinnerCode = g.GM_WinningSubmission.SBM_Code,
			WinnerIsYou = g.GM_WinningSubmission.SBM_SubmittedBy.GMP_USR_FK == userId
		}));
	}
}