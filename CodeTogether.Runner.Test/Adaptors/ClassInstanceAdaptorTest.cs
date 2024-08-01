using CodeTogether.Data.Models.Questions;
using CodeTogether.Runner.Engine;
using System.Reflection;

namespace CodeTogether.Runner.Adaptors.Test;

internal class ClassInstanceAdaptorTest
{

	ExecutionConfigurationModel CreateExecutionConfiguration()
	{
		return new ExecutionConfigurationModel()
		{
			EXE_AdapterName = "ClassInstanceAdaptor",
			EXE_AdapterArgument = "CodeTogether.Runner.Adaptors.Test.TestRunner::DoCalculation",
			EXE_ScaffoldName = string.Empty,
			EXE_ReturnArgument = ArgumentModel.FromType(typeof(int)),
			EXE_InputArguments = new ArgumentCollectionModel()
			{
				TC_Types = [ArgumentModel.FromType(typeof(int)), ArgumentModel.FromType(typeof(int))]
			}
		};
	}

	[Test]
	public void TestRunsWithPassingTest()
	{
		var assembly = Assembly.GetExecutingAssembly();

		var executionConfiguration = CreateExecutionConfiguration();

		List<TestCaseModel> testCases =
		[
			new TestCaseModel()
			{
				TST_Title = "test1",
				TST_Arguments = ["1", "2"],
				TST_ExpectedResponse = "3",
				TST_Question = null,
			},
			new TestCaseModel()
			{
				TST_Title = "test2",
				TST_Arguments = ["3", "4"],
				TST_ExpectedResponse = "7",
				TST_Question = null,
			},
		];

		var adaptor = new ClassInstanceAdaptor(assembly, executionConfiguration, testCases);

		var result = adaptor.Execute();
		Assert.That(result.Status, Is.EqualTo(ExecutionStatus.Success));
		Assert.That(result.TestRuns.ToList(), Has.Count.EqualTo(2));
	}

	[Test]
	public void TestRunsWithFailingTest()
	{
		var assembly = Assembly.GetExecutingAssembly();
		var executionConfiguration = CreateExecutionConfiguration();

		List<TestCaseModel> testCases =
		[
			new TestCaseModel()
			{
				TST_Title = "test1",
				TST_Arguments = ["1", "2"],
				TST_ExpectedResponse = "134",
				TST_Question = null,
			}
		];

		var adaptor = new ClassInstanceAdaptor(assembly, executionConfiguration, testCases);

		var result = adaptor.Execute();
		Assert.That(result.Status, Is.EqualTo(ExecutionStatus.Failure));
		Assert.That(result.TestRuns.ToList(), Has.Count.EqualTo(1));
	}
}

public class TestRunner
{
	public int DoCalculation(int a, int b)
	{
		return a + b;
	}
}