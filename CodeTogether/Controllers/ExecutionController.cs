using CodeTogether.Client.Integration.Execution;
using CodeTogether.Data;
using CodeTogether.Data.Models.Questions;
using CodeTogether.Runner.Engine;
using CodeTogether.Services.Games;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CodeTogether.Controllers;

[Route("api/execution")]
[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
public class ExecutionController(ApplicationDbContext dbContext, IExecutionEngine executionService, IGameService gameService) : Controller
{
	[HttpPost]
	[Route("execute")]
	public async Task<IActionResult> RunCode([FromBody] ExecutionRequestDTO runCodeRequest)
	{
		if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
		{
			return BadRequest("User not authenticated");
		}

		var gamePlayerModel = dbContext.GamePlayers.Where(p => p.GMP_GM_FK == runCodeRequest.GameId && p.GMP_USR_FK == userId).First();

		var question = dbContext.Questions.Where(x => x.QST_PK == runCodeRequest.QuestionId)
			.Include(q => q.QST_TestCases)
			.Include(q => q.QST_Scaffold)
			.ThenInclude(s => s.EXE_ReturnType)
			.Include(q => q.QST_Scaffold)
			.ThenInclude(s => s.EXE_Parameters)
			.ThenInclude(p => p.TC_Type)
			.FirstOrDefault();
		if (question == null)
		{
			return BadRequest("Question not found");
		}

		var result = executionService.ExecuteAgainstQuestion(question, runCodeRequest.RawCode, gamePlayerModel);
		dbContext.Add(result);

		if (result.SBM_Status == ExecutionStatus.Success)
		{
			gameService.MarkAsFinished(dbContext, runCodeRequest.GameId, result);
		}

		await dbContext.SaveChangesAsync();

		var resultDto = new ExecutionResponseDTO
		{
			State = result.SBM_Status.ToString(),
			Output = result.SBM_Status switch
			{
				ExecutionStatus.CompileError => result.SBM_CompileError ?? "Error occurred",
				ExecutionStatus.Timeout => "Tests timed out",
				_ => "Compiled Successfully"
			},
			TestResults = result.SBM_TestRuns.Select(x => new TestCaseDto.RunDTO()
			{
				Id = x.TCR_Parent.TST_PK,
				IsPassed = x.TCR_Status == TestCaseStatus.Success,
				ActualResult = x.TCR_ActualResult + (x.TCR_Exception ?? string.Empty),
			}).ToArray()
		};
		return Json(resultDto);
	}
}