using CodeTogether.Data.Models.Factories;
using CodeTogether.Data.Models.Questions;

namespace CodeTogether.Data.Seeding;

public class QuestionSeeder : ISeedStep
{
	readonly ApplicationDbContext dbContext;
	readonly ICachedTypeModelFactory typeModelFactory;

	public QuestionSeeder(ApplicationDbContext dbContext)
	{
		this.dbContext = dbContext;
		this.typeModelFactory = new CachedCachedTypeModelFactory();
	}

	public int Order { get; } = 1;

	public void Seed()
	{
		SeedSimpleAdd();
		SeedHelloWorld();
	}

	void SeedSimpleAdd()
	{
		// TODO: Add scaffold factory
		var scaffold = new ScaffoldModel
		{
			EXE_ScaffoldName = "IntIntToInt",
			EXE_ExecutionRunnerName = "ClassInstanceSubmissionExecutor",
			EXE_ReturnArgument = typeModelFactory.Get(typeof(int)),
		};

		var parameters = new List<ParameterModel>()
		{
			new () { TC_Name = "a", TC_Type = typeModelFactory.Get(typeof(int)), TC_Scaffold = scaffold },
			new () { TC_Name = "b", TC_Type = typeModelFactory.Get(typeof(int)), TC_Scaffold = scaffold }
		};

		scaffold.EXE_Parameters = parameters;

		var testCases = new TestCaseModel[]
		{
			new ()
			{
				TST_Title = "Add to Three",
				TST_Arguments = ["1","2"],
				TST_ExpectedResponse = "3",
				TST_IsHidden = false,
			},
			new ()
			{
				TST_Title = "Add Big",
				TST_Arguments = ["111","2320"],
				TST_ExpectedResponse = "2431",
				TST_IsHidden = false,
			},
			new ()
			{
				TST_Title = "Add Negative",
				TST_Arguments = ["-3","5"],
				TST_ExpectedResponse = "2",
				TST_IsHidden = false,
			},
		};

		var simpleAddQuestion = new QuestionModel()
		{
			QST_Name = QuestionModel.Constants.SimpleAdd,
			QST_Description = "Return the result of adding both arguments together",
			QST_Scaffold = scaffold,
			QST_TestCases = testCases,
		};

		dbContext.Questions.Add(simpleAddQuestion);
		dbContext.SaveChanges();
	}

	void SeedHelloWorld()
	{
		var scaffold = new ScaffoldModel()
		{
			EXE_ScaffoldName = "PureString",
			EXE_ExecutionRunnerName = "ClassInstanceSubmissionExecutor",
			EXE_ReturnArgument = typeModelFactory.Get(typeof(string))
		};

		var testCases = new TestCaseModel[]
		{
			new ()
			{
				TST_Title = "Hello World",
				TST_Arguments = [],
				TST_ExpectedResponse = "Hello World!",
				TST_IsHidden = false,
			},
		};

		var helloWorldQuestion = new QuestionModel()
		{
			QST_Name = QuestionModel.Constants.HelloWorld,
			QST_Description = "Return the string \"Hello World!\", you can do that can't you?",
			QST_Scaffold = scaffold,
			QST_TestCases = testCases
		};

		dbContext.Questions.Add(helloWorldQuestion);
		dbContext.SaveChanges();
	}
}