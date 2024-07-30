using CodeTogether.Data.Models.Questions;

namespace CodeTogether.Data.Seeding;

internal class QuestionSeeder
{
	readonly ApplicationDbContext dbContext;

	internal QuestionSeeder(ApplicationDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	public void Seed()
	{
		SeedHelloWorld();
	}

	void SeedHelloWorld()
	{
		var executionConfiguration = new ExecutionConfigurationModel(
			"HelloWorldScaffold", 
			"HelloWorld"
			);
		var helloWorldQuestion = new QuestionModel("Hello World!", "Return the string \"Hello World!\", you can do that can't you?", executionConfiguration);
		dbContext.Questions.Add(helloWorldQuestion);
		dbContext.SaveChanges();
	}
}