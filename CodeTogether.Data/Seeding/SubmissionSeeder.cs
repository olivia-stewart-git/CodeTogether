using CodeTogether.Data.Models.Questions;
using CodeTogether.Data.Models.Submission;

namespace CodeTogether.Data.Seeding;

internal class SubmissionSeeder : ISeedStep
{
	readonly ApplicationDbContext dbContext;

	public SubmissionSeeder(ApplicationDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	public void Seed()
	{
		SeedHelloWorldSubmission();
		SeedSimpleAddSubmission();

		dbContext.SaveChanges();
	}

	void SeedHelloWorldSubmission()
	{
		var helloWorldQuestion = dbContext.Questions.FirstOrDefault(x => x.QST_Name == QuestionModel.Constants.HelloWorld);
		if (helloWorldQuestion == null)
		{
			return;
		}

		var submission = new SubmissionModel()
		{
			SBM_Question = helloWorldQuestion,
			SBM_Code = @"
using System;

public class HelloWorldProblem
{
	public string HelloWorld()
	{
		return ""Hello World!"";
	}
}
",
		};

		dbContext.Submissions.Add(submission);
	}

	void SeedSimpleAddSubmission()
	{
		var simpleAddQuestion = dbContext.Questions.FirstOrDefault(x => x.QST_Name == QuestionModel.Constants.SimpleAdd);
		if (simpleAddQuestion == null)
		{
			return;
		}

		var submission = new SubmissionModel()
		{
			SBM_Question = simpleAddQuestion,
			SBM_Code = @"
using System;

public class SimpleAdd
{
	public int Add(int a, int b)
	{
		return a + b;
	}
}
",
		};

		dbContext.Submissions.Add(submission);
    }
}