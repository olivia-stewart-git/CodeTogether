using CodeTogether.Data.Models.Questions;

namespace CodeTogether.Data.Seeding;

internal class QuestionSeeder : ISeedStep
{
	readonly ApplicationDbContext dbContext;

	internal QuestionSeeder(ApplicationDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	public void Seed()
	{
		SeedSimpleAdd();
		SeedHelloWorld();
	}

	void SeedSimpleAdd()
	{
		var inputArguments = new ArgumentCollectionModel()
		{
			TC_Types =
			[
				ArgumentModel.FromType(typeof(int)),
				ArgumentModel.FromType(typeof(int)),
			]
		};

		var executionConfiguration = new ExecutionConfigurationModel
		{
			EXE_ScaffoldName = "SimpleAddScaffold",
			EXE_AdapterName = "ClassInstanceAdaptor",
			EXE_AdapterArgument = "SimpleAdd::Add",
			EXE_InputArguments = inputArguments,
			EXE_ReturnArgument = ArgumentModel.FromType(typeof(int)),
		};

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
			QST_ExecutionConfigurationModel = executionConfiguration,
			QST_TestCases = testCases,
		};

		dbContext.Questions.Add(simpleAddQuestion);
		dbContext.SaveChanges();
	}

	void SeedHelloWorld()
	{
		var inputArguments = new ArgumentCollectionModel();
		var executionConfiguration = new ExecutionConfigurationModel()
		{
			EXE_ScaffoldName = "HelloWorldScaffold",
			EXE_AdapterName = "ClassInstanceAdaptor",
			EXE_AdapterArgument = "HelloWorldProblem::HelloWorld",
			EXE_InputArguments = inputArguments,
			EXE_ReturnArgument = ArgumentModel.FromType(typeof(string))
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
			QST_ExecutionConfigurationModel = executionConfiguration,
			QST_TestCases = testCases
		};

		dbContext.Questions.Add(helloWorldQuestion);
		dbContext.SaveChanges();
	}
}