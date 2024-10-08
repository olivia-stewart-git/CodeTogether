﻿using CodeTogether.Data.Models.Questions;
using CodeTogether.Runner.Engine;
using System.Reflection;

namespace CodeTogether.Runner.Adaptors.Test;

internal class ClassInstanceSubmissionExecutorTest
{
	static ScaffoldModel CreateExecutionConfiguration()
	{
		var scaffold = new ScaffoldModel()
		{
			EXE_ExecutionRunnerName = ExecutionRunnerType.ClassInstance,
			EXE_ExecutionRunnerArgument = "CodeTogether.Runner.Adaptors.Test.TestRunner::DoCalculation",
			EXE_ScaffoldName = "scaffoldKeyForTest",
			EXE_ScaffoldText = "",
			EXE_ReturnType = TypeModel.FromType(typeof(int)),
		};

		scaffold.EXE_Parameters = new List<ParameterModel>()
		{
			new () { TC_Name = "a", TC_Position = 0, TC_Type = TypeModel.FromType(typeof(int)), TC_Scaffold = scaffold },
			new () { TC_Name = "b", TC_Position = 1, TC_Type = TypeModel.FromType(typeof(int)), TC_Scaffold = scaffold },
		};

		return scaffold;
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
				// null-forgiving operator fine in test classes
				TST_Question = null!,
			},
			new TestCaseModel()
			{
				TST_Title = "test2",
				TST_Arguments = ["3", "4"],
				TST_ExpectedResponse = "7",
				// null-forgiving operator fine in test classes
				TST_Question = null!,
			},
		];

		var adaptor = new ClassInstanceSubmissionExecutor(executionConfiguration, testCases);

		var result = adaptor.Execute(assembly);
		Assert.Multiple(() =>
		{
			Assert.That(result.Select(r => r.TCR_Status), Is.All.EqualTo(TestCaseStatus.Success));
			Assert.That(result, Has.Count.EqualTo(2));
		});
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
				// null-forgiving operator fine in test classes
				TST_Question = null!,
			}
		];

		var adaptor = new ClassInstanceSubmissionExecutor(executionConfiguration, testCases);

		var result = adaptor.Execute(assembly);
		Assert.Multiple(() =>
		{
			Assert.That(result.Select(r => r.TCR_Status).First(), Is.EqualTo(TestCaseStatus.Failure));
			Assert.That(result, Has.Count.EqualTo(1));
		});
	}
}

public class TestRunner
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Reflection reasons")]
	public int DoCalculation(int a, int b)
	{
		return a + b;
	}
}