using CodeTogether.Data.Models.Factories;
using CodeTogether.Data.Models.Questions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

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

	public void Seed(bool initialSeed)
	{
		ClearQuestionsAndSubmissions();

		SeedSimpleAdd();
		SeedHelloWorld();
	}

	public void ClearQuestionsAndSubmissions()
	{
		dbContext.Submissions.ExecuteDelete();
		dbContext.ExecutionResults.ExecuteDelete();
		dbContext.TestRuns.ExecuteDelete();
		dbContext.TestExecutions.ExecuteDelete();
		dbContext.TestCases.ExecuteDelete();
		dbContext.Questions.ExecuteDelete();
		dbContext.ExecutionConfigurations.ExecuteDelete();
		dbContext.ArgumentTypes.ExecuteDelete();
		dbContext.QuestionSignatures.ExecuteDelete();
	}

	void SeedSimpleAdd()
	{
		var inputArguments = new QuestionSignatureModel()
		{
			TC_Types =
			[
				typeModelFactory.Get(typeof(int)),
				typeModelFactory.Get(typeof(int)),
			]
		};

		var executionConfiguration = new ExecutionConfigurationModel
		{
			EXE_ScaffoldName = "SimpleAddScaffold",
			EXE_ExecutionRunnerName = "ClassInstanceSubmissionExecutor",
			EXE_ExecutionArgument = "SimpleAdd::Add",
			EXE_InputArguments = inputArguments,
			EXE_ReturnArgument = typeModelFactory.Get(typeof(int)),
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
		var inputArguments = new QuestionSignatureModel();
		var executionConfiguration = new ExecutionConfigurationModel()
		{
			EXE_ScaffoldName = "HelloWorldScaffold",
			EXE_ExecutionRunnerName = "ClassInstanceSubmissionExecutor",
			EXE_ExecutionArgument = "HelloWorldProblem::HelloWorld",
			EXE_InputArguments = inputArguments,
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
			QST_ExecutionConfigurationModel = executionConfiguration,
			QST_TestCases = testCases
		};

		dbContext.Questions.Add(helloWorldQuestion);
		dbContext.SaveChanges();
	}
}