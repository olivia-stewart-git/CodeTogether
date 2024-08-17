using CodeTogether.Client.Integration.Execution;
using CodeTogether.Data;
using CodeTogether.Data.Models.Questions;
using CodeTogether.Runner.Engine;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeTogether.Controllers;

[Route("api/execution")]
public class ExecutionController : Controller
{
	readonly ApplicationDbContext dbContext;
	readonly IExecutionEngine executionService;

	public ExecutionController(ApplicationDbContext dbContext, IExecutionEngine executionService)
	{
		this.dbContext = dbContext;
		this.executionService = executionService;
	}

	[HttpPost]
	[Route("execute")]
	public async Task<IActionResult> RunCode([FromBody] ExecutionRequestDTO runCodeRequest)
	{
		var user = User.Identity?.Name;
		if (string.IsNullOrEmpty(user))
		{
			return BadRequest("User not authenticated");
		}

		var question = dbContext.Questions.Where(x => x.QST_PK == runCodeRequest.QuestionId)
			.Include(x => x.QST_TestCases)
			.Include(x => x.QST_Scaffold)
			.ThenInclude(x => x.EXE_Parameters)
			.FirstOrDefault();
		if (question == null)
		{
			return BadRequest("Question not found");
		}

		var result = executionService.ExecuteAgainstQuestion(question, runCodeRequest.RawCode);

		var completedSubmission = new CompletedSubmissionModel
		{
			CSM_CompletedAt = DateTime.Now,
			CSM_Code = runCodeRequest.RawCode,
			CSM_USR_FK = runCodeRequest.UserId,
			CSM_Result = result,
			CSM_GM_FK = runCodeRequest.GameId
		};
		dbContext.CompletedSubmissions.Add(completedSubmission);
		await dbContext.SaveChangesAsync();

		var resultDto = new ExecutionResponseDTO
		{
			State = result.EXR_Status.ToString(),
			Output = result.EXR_Status switch
			{
				ExecutionStatus.Error => result.EXR_CompileError?.Message ?? "Error occurred",
				_ => "Compiled Successfully"
			},
			TestResults = result.EXR_TestRuns.Select(x => new TestCaseDto.RunDTO()
			{
				Id = x.TCR_Parent.TST_PK,
				IsPassed = x.TCR_Status == TestCaseStatus.Success,
				ActualResult = x.TCR_ActualResult + (x.TCR_Exception?.Message ?? string.Empty),
			}).ToArray()
		};
		return Json(resultDto);
	}
}