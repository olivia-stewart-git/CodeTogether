using CodeTogether.Client.Integration;
using CodeTogether.Client.Integration.Execution;
using CodeTogether.Data;
using CodeTogether.Services.Games;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace CodeTogether.Controllers;

[Route("api/question")]
public class QuestionController(ApplicationDbContext dbContext, QuestionService questionService) : Controller
{
	/// <summary>
	/// for now just get first question in db
	/// </summary>
	/// <param name="gameId"></param>
	/// <returns></returns>
	[Route("get/{gameId}")]
	public IActionResult GetQuestion(Guid gameId)
	{
		var questionId = dbContext.Games.Find(gameId)?.GM_QST_FK ?? throw new InvalidOperationException("No game or no question");

		var question = questionService.GetQuestionById(questionId);
		if (question == null)
		{
			return BadRequest("Question does not exist");
		}
		return Json(question);
	}

	[Route("list")]
	public IActionResult AllQuestions()
	{
		return Json(dbContext.Questions.Select(x => new QuestionListQuestionDTO { Name = x.QST_Name, Id = x.QST_PK }));
	}
}