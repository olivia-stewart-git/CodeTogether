using CodeTogether.Client.Integration.Execution;
using CodeTogether.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeTogether.Controllers;

[Route("api/question")]
public class QuestionController : Controller
{
	readonly ApplicationDbContext dbContext;

	public QuestionController(ApplicationDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	/// <summary>
	/// for now just get first question in db
	/// </summary>
	/// <param name="gameId"></param>
	/// <returns></returns>
	[Route("get/{gameId}")]
	public IActionResult GetQuestion(Guid gameId)
	{
		var question = dbContext.Questions
			.Take(1)
			.Include(x => x.QST_Scaffold)
			.Include(x => x.QST_TestCases)
			.FirstOrDefault();

		if (question == null)
		{
			return NotFound("No question found");
		}

		var questionDTO = new QuestionDTO
		{
			Id = question.QST_PK,
			Name = question.QST_Name,
			Description = question.QST_Description,
			ScaffoldCode = question.QST_Scaffold.EXE_ScaffoldText,
			TestCases = question.QST_TestCases.Select(x => new TestCaseDto
			{
				Id = x.TST_PK,
				Name = x.TST_Title,
				Arguments = x.TST_Arguments,
				ExpectedResponse = x.TST_ExpectedResponse, 
			}).ToList()
		};
		return Json(questionDTO);
	}
}