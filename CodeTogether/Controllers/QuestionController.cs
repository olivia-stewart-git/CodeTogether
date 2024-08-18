using CodeTogether.Client.Integration;
using CodeTogether.Client.Integration.Execution;
using CodeTogether.Data;
using CodeTogether.Data.Models.Factories;
using CodeTogether.Data.Models.Questions;
using CodeTogether.Data.Seeding;
using CodeTogether.Services.Games;
using Microsoft.AspNetCore.Mvc;

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

	[HttpPost]
	[Route("create")]
	public IActionResult CreateQuestion([FromBody] CreateQuestionRequestDTO createRequest)
	{
		var typeModelFactory = new CachedCachedTypeModelFactory();
		var scaffoldModelFactory = new ScaffoldModelFactory(dbContext, typeModelFactory);

		var returnType = GetTypeFromInputString(createRequest.ReturnType);
		var types = createRequest.Arguments.Select(x => (x.Item1, GetTypeFromInputString(x.Item2))).ToArray();

		var scaffold = scaffoldModelFactory.GetScaffold(types.Select(x => new ParameterInfo(x.Item1, x.Item2)).ToList(), returnType, ExecutionRunnerType.ClassInstance);
		var questionModel = new QuestionModel
		{
			QST_Name = createRequest.Name,
			QST_Description = createRequest.Description,
			QST_Scaffold = scaffold
		};
		questionModel.QST_TestCases = createRequest.TestCases.Select(x => new TestCaseModel()
		{
			TST_Question = questionModel,
			TST_Title = x.Name,
			TST_ExpectedResponse = x.ExpectedResponse,
			TST_Arguments = x.Arguments
		}).ToList();

		dbContext.Questions.Add(questionModel);
		dbContext.SaveChanges();

		var loader = new QuestionLoader();
		loader.SaveQuestion(questionModel);

		return Ok();
	}

	Type GetTypeFromInputString(string input)
	{
		var type = input switch {
			"int" => typeof(int),
			"string" => typeof(string),
			"bool" => typeof(bool),
			"float" => typeof(float),
			_ => throw new InvalidOperationException("Invalid type")
		};
		return type;
	}
}