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

		SeedTwoSum();
		SeedPalindrome();
		SeedPrefix();
		SeedParens();
		SeedGreatestLetter();
		SeedDigitSum();
		SeedPivotNumber();

		SeedFromQuestionFiles();
	}

	public void ClearQuestionsAndSubmissions()
	{
		dbContext.Submissions.ExecuteDelete();
		dbContext.TestRuns.ExecuteDelete();
		dbContext.TestCases.ExecuteDelete();
		dbContext.Questions.ExecuteDelete();
		dbContext.Scaffolds.ExecuteDelete();
		dbContext.Parameters.ExecuteDelete();
		dbContext.Types.ExecuteDelete();
	}

	void SeedTwoSum()
	{
		ParameterInfo[] parameters =
		[
			new ParameterInfo("nums", typeof(List<int>)),
			new ParameterInfo("target", typeof(int)),
		];

		var scaffold = scaffoldModelFactory.GetScaffold(parameters, typeof((int, int)));

		var twoSumQuestion = new QuestionModel()
		{
			QST_Name = "Two Sum",
			QST_Description = @"Given a list of number and a target, find the inidicies of the two numbers that add up to target, there will only be one valid pair and the returned tuple should have the smaller of the two first.",
			QST_Scaffold = scaffold,
		};

		var size = 1000;
		var nums = new List<int>();
		for (int i = 0; i < size; i++)
		{
			// over 2 to avoid overflows
			// relying on the ostrich algorithm to ensure there is only once answer
			nums.Add(Random.Shared.Next(int.MinValue / 2, int.MaxValue / 2));
		}

		var idx1 = Random.Shared.Next(size);
		var idx2 = Random.Shared.Next(size);
		while (idx1 == idx2)
		{
			idx2 = Random.Shared.Next(size);
		}
		if (idx1 > idx2)
		{
			(idx1, idx2) = (idx2, idx1);
		}
		var target = nums[idx1] + nums[idx2];
		var numsString = string.Join(", ", nums.Select(p => p.ToString()));

		twoSumQuestion.QST_TestCases = new TestCaseModel[]
		{
			new ()
			{
				TST_Title = "Test 1",
				TST_Arguments = ["[11, 2, 15, 7]", "9"],
				TST_ExpectedResponse = "[1, 3]",
				TST_IsHidden = false,
				TST_Question = twoSumQuestion,
			},
			new ()
			{
				TST_Title = "Test 2",
				TST_Arguments = ["[3, 2, 4]", "6"],
				TST_ExpectedResponse = "[1, 2]",
				TST_IsHidden = false,
				TST_Question = twoSumQuestion,
			},
			new ()
			{
				TST_Title = "Test 3",
				TST_Arguments = ["[3, 3]", "6"],
				TST_ExpectedResponse = "[0, 1]",
				TST_IsHidden = false,
				TST_Question = twoSumQuestion,
			},
			new ()
			{
				TST_Title = "Test 4",
				TST_Arguments = [$"[{numsString}]", target.ToString()],
				TST_ExpectedResponse = $"[{idx1}, {idx2}]",
				TST_IsHidden = true,
				TST_Question = twoSumQuestion,
			},
		};

		dbContext.Questions.Add(twoSumQuestion);
		dbContext.SaveChanges();
	}

	void SeedPalindrome()
	{
		var scaffold = scaffoldModelFactory.GetScaffold(new ParameterInfo[] { new("x", typeof(int))}, typeof(bool));

		var question = new QuestionModel()
		{
			QST_Name = "Palindrome",
			QST_Description = "Return true if the given number is a palindrome, false otherwise.",
			QST_Scaffold = scaffold,
		};

		question.QST_TestCases = new TestCaseModel[]
		{
			new ()
			{
				TST_Title = "Test 1",
				TST_Arguments = ["121"],
				TST_ExpectedResponse = "true",
				TST_IsHidden = false,
				TST_Question = question,
			},
			new ()
			{
				TST_Title = "Test 2",
				TST_Arguments = ["-121"],
				TST_ExpectedResponse = "false",
				TST_IsHidden = false,
				TST_Question = question,
			},
			new ()
			{
				TST_Title = "Test 3",
				TST_Arguments = ["3"],
				TST_ExpectedResponse = "true",
				TST_IsHidden = false,
				TST_Question = question,
			},
			new ()
			{
				TST_Title = "Test 4",
				TST_Arguments = ["01"],
				TST_ExpectedResponse = "true",
				TST_IsHidden = false,
				TST_Question = question,
			},
			new ()
			{
				TST_Title = "",
				TST_Arguments = ["00100"],
				TST_ExpectedResponse = "true",
				TST_IsHidden = true,
				TST_Question = question,
			},
			new ()
			{
				TST_Title = "",
				TST_Arguments = ["001100"],
				TST_ExpectedResponse = "true",
				TST_IsHidden = true,
				TST_Question = question,
			},
			new ()
			{
				TST_Title = "",
				TST_Arguments = ["1234567890987654321"],
				TST_ExpectedResponse = "true",
				TST_IsHidden = true,
				TST_Question = question,
			},
			new ()
			{
				TST_Title = "",
				TST_Arguments = ["12345678909876543211"],
				TST_ExpectedResponse = "false",
				TST_IsHidden = true,
				TST_Question = question,
			},
		};


		dbContext.Questions.Add(question);
		dbContext.SaveChanges();
	}

	void SeedPrefix()
	{
		ParameterInfo[] parameters =
		[
			new ParameterInfo("strs", typeof(List<string>)),
		];

		var scaffold = scaffoldModelFactory.GetScaffold(parameters, typeof(string));

		var question = new QuestionModel()
		{
			QST_Name = "Common Prefix",
			QST_Description = "Write a function to find the longest common prefix string amongst an array of strings, or an empty string if there is no common prefix.",
			QST_Scaffold = scaffold,
		};

		question.QST_TestCases = new TestCaseModel[]
		{
			new ()
			{
				TST_Title = "Has a common prefix",
				TST_Arguments = ["[\"flower\",\"flow\",\"flight\"]", "\"fl\""],
				TST_ExpectedResponse = "fl",
				TST_IsHidden = false,
				TST_Question = question,
			},
			new ()
			{
				TST_Title = "No Common Prefix",
				TST_Arguments = ["[\"dog\",\"racecar\",\"car\"]"],
				TST_ExpectedResponse = "",
				TST_IsHidden = false,
				TST_Question = question,
			},
			new ()
			{
				TST_Title = "",
				TST_Arguments = ["[\"apple\", \"applet\", \"applote\", \"applyer\"]"],
				TST_ExpectedResponse = "appl",
				TST_IsHidden = true,
				TST_Question = question,
			},
			new ()
			{
				TST_Title = "Empty",
				TST_Arguments = ["[]"],
				TST_ExpectedResponse = "",
				TST_IsHidden = true,
				TST_Question = question,
			},
		};

		dbContext.Questions.Add(question);
		dbContext.SaveChanges();
	}

	void SeedParens()
	{
		ParameterInfo[] parameters =
		[
			new ParameterInfo("s", typeof(string)),
		];

		var scaffold = scaffoldModelFactory.GetScaffold(parameters, typeof(bool));

		var question = new QuestionModel()
		{
			QST_Name = "Valid Parentheses",
			QST_Description = "Given a string s containing just the characters '(){}[]', determine if the input string is valid. Parentheses must all be closed and must be closed in the same order they are opened",
			QST_Scaffold = scaffold,
		};

		question.QST_TestCases = new TestCaseModel[]
		{
			new ()
			{
				TST_Title = "Closed in wrong order",
				TST_Arguments = ["([)]"],
				TST_ExpectedResponse = "false",
				TST_IsHidden = false,
				TST_Question = question,
			},
			new ()
			{
				TST_Title = "Valid",
				TST_Arguments = ["({}[])"],
				TST_ExpectedResponse = "true",
				TST_IsHidden = false,
				TST_Question = question,
			},
			new ()
			{
				TST_Title = "",
				TST_Arguments = ["{()()()()()()()()()([[]])}"],
				TST_ExpectedResponse = "true",
				TST_IsHidden = true,
				TST_Question = question,
			},
			new ()
			{
				TST_Title = "Empty",
				TST_Arguments = ["((((((((([[[[[[[[)))))))))"],
				TST_ExpectedResponse = "false",
				TST_IsHidden = true,
				TST_Question = question,
			},
		};

		dbContext.Questions.Add(question);
		dbContext.SaveChanges();
	}

	void SeedGreatestLetter()
	{
		ParameterInfo[] parameters =
		[
			new ParameterInfo("s", typeof(string)),
		];

		var scaffold = scaffoldModelFactory.GetScaffold(parameters, typeof(char));

		var question = new QuestionModel()
		{
			QST_Name = "Greatest Letter",
			QST_Description = @"Given a string of English letters s, return the greatest English letter which occurs as both a lowercase and uppercase letter in s. The returned letter should be in uppercase. If no such letter exists, return a space.

An English letter b is greater than another letter a if b appears after a in the English alphabet.",
			QST_Scaffold = scaffold,
		};

		question.QST_TestCases = new TestCaseModel[]
		{
			new ()
			{
				TST_Title = "T is greater than O",
				TST_Arguments = ["COdeZTogether"],
				TST_ExpectedResponse = "T",
				TST_IsHidden = false,
				TST_Question = question,
			},
			new ()
			{
				TST_Title = "None",
				TST_Arguments = ["codetogether"],
				TST_ExpectedResponse = " ",
				TST_IsHidden = false,
				TST_Question = question,
			},
			new ()
			{
				TST_Title = "",
				TST_Arguments = ["asdfjklAsdfjlASDFJKLzxcvbnm"],
				TST_ExpectedResponse = "Z",
				TST_IsHidden = true,
				TST_Question = question,
			},
			new ()
			{
				TST_Title = "Empty",
				TST_Arguments = ["abcdefgabcdefgabcdefgabcdefgabcdefgabcdefgABCDEFGABCDEFGABCDEFGABCDEFGABCDEFGABCDEFGzzzzABCDEFGABCDEFGabcdefgYyzz"],
				TST_ExpectedResponse = "Y",
				TST_IsHidden = true,
				TST_Question = question,
			},
		};

		dbContext.Questions.Add(question);
		dbContext.SaveChanges();
	}

	void SeedPivotNumber()
	{
		ParameterInfo[] parameters =
		[
			new ParameterInfo("n", typeof(int)),
		];

		var scaffold = scaffoldModelFactory.GetScaffold(parameters, typeof(int));

		var question = new QuestionModel()
		{
			QST_Name = "Pivot Number",
			QST_Description = @"Given a positive integer n, find the pivot integer x such that The sum of all elements between 1 and x inclusively equals the sum of all elements between x and n inclusively.
Return the pivot integer x. If no such integer exists, return -1. It is guaranteed that there will be at most one pivot index for the given input.",
			QST_Scaffold = scaffold,
		};

		question.QST_TestCases = new TestCaseModel[]
		{
			new ()
			{
				TST_Title = "Eight",
				TST_Arguments = ["8"],
				TST_ExpectedResponse = "6",
				TST_IsHidden = false,
				TST_Question = question,
			},
			new ()
			{
				TST_Title = "One",
				TST_Arguments = ["1"],
				TST_ExpectedResponse = "1",
				TST_IsHidden = false,
				TST_Question = question,
			},
			new ()
			{
				TST_Title = "None",
				TST_Arguments = ["4"],
				TST_ExpectedResponse = "-1",
				TST_IsHidden = false,
				TST_Question = question,
			},
			new ()
			{
				TST_Title = "",
				TST_Arguments = ["49"],
				TST_ExpectedResponse = "35",
				TST_IsHidden = true,
				TST_Question = question,
			},
			new ()
			{
				TST_Title = "",
				TST_Arguments = ["20"],
				TST_ExpectedResponse = "-1",
				TST_IsHidden = true,
				TST_Question = question,
			},
		};

		dbContext.Questions.Add(question);
		dbContext.SaveChanges();
	}

	void SeedDigitSum()
	{
		ParameterInfo[] parameters =
		[
			new ParameterInfo("n", typeof(int)),
		];

		var scaffold = scaffoldModelFactory.GetScaffold(parameters, typeof(int));

		var question = new QuestionModel()
		{
			QST_Name = "Alternative Digit Sum",
			QST_Description = @"You are given a positive integer n. Each digit of n has a sign according to the following rules:
The most significant digit is assigned a positive sign.
Each other digit has an opposite sign to its adjacent digits.
Return the sum of all digits.",
			QST_Scaffold = scaffold,
		};

		question.QST_TestCases = new TestCaseModel[]
		{
			new ()
			{
				TST_Title = "521",
				TST_Arguments = ["521"],
				TST_ExpectedResponse = "4",
				TST_IsHidden = false,
				TST_Question = question,
			},
			new ()
			{
				TST_Title = "111",
				TST_Arguments = ["111"],
				TST_ExpectedResponse = "1",
				TST_IsHidden = false,
				TST_Question = question,
			},
			new ()
			{
				TST_Title = "886996",
				TST_Arguments = ["886996"],
				TST_ExpectedResponse = "0",
				TST_IsHidden = true,
				TST_Question = question,
			},
			new ()
			{
				TST_Title = "",
				TST_Arguments = ["123456"],
				TST_ExpectedResponse = "-3",
				TST_IsHidden = true,
				TST_Question = question,
			},
			new ()
			{
				TST_Title = "",
				TST_Arguments = ["90909"],
				TST_ExpectedResponse = "27",
				TST_IsHidden = true,
				TST_Question = question,
			},
		};

		dbContext.Questions.Add(question);
		dbContext.SaveChanges();
	}

	void SeedFromQuestionFiles()
	{
		var loader = new QuestionLoader();
		var questions = loader.LoadQuestions();
		foreach (var question in questions)
		{
			dbContext.Questions.Add(question);
		}
		dbContext.SaveChanges();
	}
}