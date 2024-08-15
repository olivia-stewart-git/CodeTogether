using CodeTogether.Data.Models.Questions;
using CodeTogether.Runner.Adaptors;
using CodeTogether.Runner.Scaffolds;
using Moq;
using System.Text.Json;
using TestNameSpace;

namespace CodeTogether.Runner.Engine.Test
{
	public class ExecutionEngineTest
	{
		[Test]
		public void TestCatchesCompilationException()
		{
			const string code = @"
public class TestClass 
invalid code !!!!!
}
";

			var executorFactory = new ExecutorFactory();
			var compilationEngine = new CompilationEngine();
			var engine = new ExecutionEngine(compilationEngine, executorFactory, scaffoldLoaderMock.Object);

			var question = new QuestionModel()
			{
				QST_Name = "Test question",
				QST_Description = "Test",
				QST_TestCases = new TestCaseModel[]
				{
					new()
					{
						TST_Title = "adding 1 and 2",
						TST_ExpectedResponse = "1",
						TST_IsHidden = false,
						TST_Arguments = ["1", "2"]
					}
				},
				QST_Scaffold = new()
				{
					EXE_ExecutionRunnerName = "ClassInstanceSubmissionExecutor",
					EXE_ScaffoldName = "testScaffold",
					EXE_ExecutionArgument = "TestClass::TestMethod",
					EXE_ReturnType = TypeModel.FromType(typeof(int)),
					EXE_Parameters = new ParameterModel()
					{
						TC_Types = [TypeModel.FromType(typeof(int)), TypeModel.FromType(typeof(int))]
					}
				}
			};

			var result = engine.ExecuteAgainstQuestion(question, code);
			Assert.That(result.EXR_Status, Is.EqualTo(ExecutionStatus.Error));
			Assert.NotNull(result.EXR_CompileError);
			Assert.That(result.EXR_CompileError.GetType(), Is.EqualTo(typeof(CompilationException)));
		}

		[Test]
		public void TestRunsWithJsonResponse()
		{
			const string returnName = "test";
			const string returnValue = "testVal";
			const string code = @"
using TestNameSpace;

public class TestClass 
{
	public TestJsonObject TestMethod(string name, string value) 
	{
		return new TestJsonObject(name, value);
	}
}
";
			scaffoldLoaderMock.Setup(x => x.LoadScaffold("testScaffold"))
				.Returns(new Scaffold(string.Empty, [typeof(TestJsonObject)]));

			var executorFactory = new ExecutorFactory();
			var compilationEngine = new CompilationEngine();
			var engine = new ExecutionEngine(compilationEngine, executorFactory, scaffoldLoaderMock.Object);

			var question = new QuestionModel()
			{
				QST_Name = "Test question",
				QST_Description = "Test",
				QST_TestCases = new TestCaseModel[]
				{
					new()
					{
						TST_Title = "return my object",
						TST_ExpectedResponse = JsonSerializer.Serialize(new TestJsonObject(returnName, returnValue)),
						TST_IsHidden = false,
						TST_Arguments = [returnName, returnValue]
					}
				},
				QST_Scaffold = new()
				{
					EXE_ExecutionRunnerName = "ClassInstanceSubmissionExecutor",
					EXE_ExecutionArgument = "TestClass::TestMethod",
					EXE_ScaffoldName = "testScaffold",
					EXE_ReturnType = TypeModel.FromType(typeof(TestJsonObject)),
					EXE_Parameters = new ParameterModel()
					{
						TC_Types = [TypeModel.FromType(typeof(string)), TypeModel.FromType(typeof(string))]
					}
				}
			};

			var result = engine.ExecuteAgainstQuestion(question, code);
			Assert.That(result.EXR_Status, Is.EqualTo(ExecutionStatus.Success));
		}

		[TestCase("3", ExecutionStatus.Success, "3")]
		[TestCase("18", ExecutionStatus.Failure, "3")]
		public void TestExecutionSuccess(string returnValue, ExecutionStatus expectedStatus, string expectedActualResult)
		{
			const string code = @"
using System;

public class TestClass 
{
	public int TestMethod(int a, int b) 
	{
		return a + b;
	}
}
";

			var executorFactory = new ExecutorFactory();
			var compilationEngine = new CompilationEngine();
			var engine = new ExecutionEngine(compilationEngine, executorFactory, scaffoldLoaderMock.Object);

			var question = new QuestionModel()
			{
				QST_Name = "Test question",
				QST_Description = "Test",
				QST_TestCases = new TestCaseModel[]
				{
					new()
					{
						TST_Title = "adding 1 and 2",
						TST_ExpectedResponse = returnValue,
						TST_IsHidden = false,
						TST_Arguments = ["1", "2"]
					}
				},
				QST_Scaffold = new ()
				{
					EXE_ExecutionRunnerName = "ClassInstanceSubmissionExecutor",
					EXE_ExecutionArgument = "TestClass::TestMethod",
					EXE_ReturnType = TypeModel.FromType(typeof(int)),
					EXE_Parameters = new ParameterModel()
					{
						TC_Types = [TypeModel.FromType(typeof(int)), TypeModel.FromType(typeof(int))]
					}
				}
			};

			var result = engine.ExecuteAgainstQuestion(question, code);
			Assert.That(result.EXR_Status, Is.EqualTo(expectedStatus));
			Assert.That(result.EXR_TestRuns!.TRX_TestRuns[0].TCR_ActualResult, Is.EqualTo(expectedActualResult));
		}

		Mock<IScaffoldLoader> scaffoldLoaderMock;
		[SetUp]
		public void Setup()
		{
			scaffoldLoaderMock = new Mock<IScaffoldLoader>();
			scaffoldLoaderMock.Setup(x => x.LoadScaffold(It.IsAny<string>())).Returns(new Scaffold(string.Empty, []));
		}
	}
}

namespace TestNameSpace
{
	public struct TestJsonObject
	{
		public TestJsonObject(string name, string value)
		{
			Name = name;
			Value = value;
		}
		public string Name { get; set; }
		public string Value { get; set; }
	}
}