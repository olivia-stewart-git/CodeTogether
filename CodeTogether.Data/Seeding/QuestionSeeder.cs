using CodeTogether.Data.Models.Factories;
using CodeTogether.Data.Models.Questions;
using Microsoft.EntityFrameworkCore;

namespace CodeTogether.Data.Seeding;

public class QuestionSeeder : ISeedStep
{
	readonly ApplicationDbContext dbContext;
	readonly ICachedTypeModelFactory typeModelFactory;
	readonly IScaffoldModelFactory scaffoldModelFactory;

	public QuestionSeeder(ApplicationDbContext dbContext)
	{
		this.dbContext = dbContext;
		typeModelFactory = new CachedCachedTypeModelFactory();
		scaffoldModelFactory = new ScaffoldModelFactory(dbContext, typeModelFactory);
	}

	public int Order { get; } = 1;

	public void Seed(bool initialSeed)
	{
		ClearQuestionsAndSubmissions();

		SeedSimpleAdd();
		SeedHelloWorld();
	}

	public void ClearQuestionsAndSubmissions()
	{
		dbContext.Submissions.ExecuteDelete();
		dbContext.SubmissionResults.ExecuteDelete();
		dbContext.TestRuns.ExecuteDelete();
		dbContext.TestCases.ExecuteDelete();
		dbContext.Questions.ExecuteDelete();
		dbContext.Scaffolds.ExecuteDelete();
		dbContext.Parameters.ExecuteDelete();
		dbContext.Types.ExecuteDelete();
	}

	void SeedSimpleAdd()
	{
		ParameterInfo[] parameters =
		[
			new ParameterInfo("a", typeof(int)),
			new ParameterInfo("b", typeof(int)),
		];

		var scaffold = scaffoldModelFactory.GetScaffold(parameters, typeof(int));

		var simpleAddQuestion = new QuestionModel()
		{
			QST_Name = "Simple Add",
			QST_Description = "Return the result of adding both arguments together",
			QST_Scaffold = scaffold,
		};

		simpleAddQuestion.QST_TestCases = new TestCaseModel[]
		{
			new ()
			{
				TST_Title = "Add to Three",
				TST_Arguments = ["1","2"],
				TST_ExpectedResponse = "3",
				TST_IsHidden = false,
				TST_Question = simpleAddQuestion,
			},
			new ()
			{
				TST_Title = "Add Big",
				TST_Arguments = ["111","2320"],
				TST_ExpectedResponse = "2431",
				TST_IsHidden = false,
				TST_Question = simpleAddQuestion,
			},
			new ()
			{
				TST_Title = "Add Negative",
				TST_Arguments = ["-3","5"],
				TST_ExpectedResponse = "2",
				TST_IsHidden = false,
				TST_Question = simpleAddQuestion,
			},
		};

		dbContext.Questions.Add(simpleAddQuestion);
		dbContext.SaveChanges();
	}

	void SeedHelloWorld()
	{
		var scaffold = scaffoldModelFactory.GetScaffold(Array.Empty<ParameterInfo>(), typeof(string));

		var helloWorldQuestion = new QuestionModel()
		{
			QST_Name = "Hello World",
			QST_Description = "Return the string \"Hello World!\", you can do that can't you?",
			QST_Scaffold = scaffold,
		};

		helloWorldQuestion.QST_TestCases = new TestCaseModel[]
		{
			new ()
			{
				TST_Title = "Hello World",
				TST_Arguments = [],
				TST_ExpectedResponse = "Hello World!",
				TST_IsHidden = false,
				TST_Question = helloWorldQuestion,
			},
			new ()
			{
				TST_Title = "Hello World2",
				TST_Arguments = [],
				TST_ExpectedResponse = "Hello World!",
				TST_IsHidden = false,
				TST_Question = helloWorldQuestion,
			},
		};


		dbContext.Questions.Add(helloWorldQuestion);
		dbContext.SaveChanges();
	}
}