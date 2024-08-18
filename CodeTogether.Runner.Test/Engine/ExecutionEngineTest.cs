using CodeTogether.Data.Models.Questions;
using CodeTogether.Runner.Adaptors;
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
			var engine = new ExecutionEngine(compilationEngine, executorFactory);

			var scaffold = new ScaffoldModel()
			{
				EXE_ExecutionRunnerName = ExecutionRunnerType.ClassInstance,
				EXE_ScaffoldName = "testScaffold",
				EXE_ScaffoldText = "whatever",
				EXE_ExecutionRunnerArgument = "TestClass::TestMethod",
				EXE_ReturnType = TypeModel.FromType(typeof(int)),
				EXE_Parameters = new ParameterModel[] { }
			};
			var question = new QuestionModel()
			{
				QST_Name = "Test question",
				QST_Description = "Test",
				QST_Scaffold = scaffold
			};

			var testCases = new TestCaseModel[]
			{
				new()
				{
					TST_Title = "adding 1 and 2",
					TST_ExpectedResponse = "1",
					TST_IsHidden = false,
					TST_Arguments = ["1", "2"],
					TST_Question = question
				}
			};
			question.QST_TestCases = testCases;

			var result = engine.ExecuteAgainstQuestion(question, code, null!);
			Assert.That(result.SBM_Status, Is.EqualTo(ExecutionStatus.Error));
			Assert.NotNull(result.SBM_CompileError);
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
			var executorFactory = new ExecutorFactory();
			var compilationEngine = new CompilationEngine();
			var engine = new ExecutionEngine(compilationEngine, executorFactory);

			var scaffold = new ScaffoldModel()
			{
				EXE_ExecutionRunnerName = ExecutionRunnerType.ClassInstance,
				EXE_ExecutionRunnerArgument = "TestClass::TestMethod",
				EXE_ScaffoldName = "testScaffold",
				EXE_ScaffoldText = "whatever",
				EXE_ReturnType = TypeModel.FromType(typeof(TestJsonObject)),
			};
			scaffold.EXE_Parameters = new ParameterModel[]
			{
				new () { TC_Name = "name", TC_Position = 0, TC_Type = TypeModel.FromType(typeof(string)), TC_Scaffold = scaffold },
				new () { TC_Name = "value", TC_Position = 1, TC_Type = TypeModel.FromType(typeof(string)), TC_Scaffold = scaffold },
			};

			var question = new QuestionModel()
			{
				QST_Name = "Test question",
				QST_Description = "Test",
				QST_Scaffold = scaffold
			};

			question.QST_TestCases = new TestCaseModel[]
			{
				new ()
				{
					TST_Title = "return my object",
					TST_ExpectedResponse = JsonSerializer.Serialize(new TestJsonObject(returnName, returnValue)),
					TST_IsHidden = false,
					TST_Arguments = [returnName, returnValue],
					TST_Question = question
				}
			};

			var result = engine.ExecuteAgainstQuestion(question, code, null!);
			Assert.That(result.SBM_CompileError, Is.EqualTo(null));
			Assert.That(result.SBM_Status, Is.EqualTo(ExecutionStatus.Success));
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
			var engine = new ExecutionEngine(compilationEngine, executorFactory);

			// TODO: single setup for this repeated model creation
			var scaffold = new ScaffoldModel
			{
				EXE_ScaffoldName = "testScaffold",
				EXE_ScaffoldText = "whatever",
				EXE_ExecutionRunnerName = ExecutionRunnerType.ClassInstance,
				EXE_ExecutionRunnerArgument = "TestClass::TestMethod",
				EXE_ReturnType = TypeModel.FromType(typeof(int)),
			};
			scaffold.EXE_Parameters = new ParameterModel[]
			{
				new () { TC_Name = "a", TC_Position = 0, TC_Type = TypeModel.FromType(typeof(int)), TC_Scaffold = scaffold },
				new () { TC_Name = "b", TC_Position = 1, TC_Type = TypeModel.FromType(typeof(int)), TC_Scaffold = scaffold },
			};

			var question = new QuestionModel()
			{
				QST_Name = "Test question",
				QST_Description = "Test",
				QST_Scaffold = scaffold
			};

			question.QST_TestCases = new TestCaseModel[]
			{
				new()
				{
					TST_Title = "adding 1 and 2",
					TST_ExpectedResponse = returnValue,
					TST_IsHidden = false,
					TST_Arguments = ["1", "2"],
					TST_Question = question
				}
			};

			var result = engine.ExecuteAgainstQuestion(question, code, null!);
			Assert.That(result.SBM_Status, Is.EqualTo(expectedStatus));
			Assert.That(result.SBM_TestRuns.First().TCR_ActualResult, Is.EqualTo(expectedActualResult));
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