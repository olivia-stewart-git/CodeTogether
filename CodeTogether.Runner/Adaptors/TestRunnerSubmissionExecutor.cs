using CodeTogether.Data.Models.Questions;
using CodeTogether.Runner.Engine;
using System.Reflection;
using TestCaseStatus = CodeTogether.Data.Models.Questions.TestCaseStatus;

namespace CodeTogether.Runner.Adaptors;

public abstract class TestRunnerSubmissionExecutor : ISubmissionExecutor
{
	protected readonly ScaffoldModel scaffold;
	readonly IEnumerable<TestCaseModel> testCases;

	protected TestRunnerSubmissionExecutor(ScaffoldModel scaffold, IEnumerable<TestCaseModel> testCases)
	{
		this.scaffold = scaffold;
		this.testCases = testCases;
	}

	public abstract object? GetExecutionResult(Assembly targetAssembly, object[] testCaseArguments);

	public SubmissionResultModel Execute(Assembly targetAssembly)
	{
		List<TestRunModel> testRuns = [];
		var submissionResult = new SubmissionResultModel { EXR_Status = ExecutionStatus.InProgress };
		foreach (var testCaseModel in testCases)
		{
			try
			{
				var arguments = GetTestCaseArguments(testCaseModel);
				var result = GetExecutionResult(targetAssembly, arguments);
				var testRun = AssertTestCase(testCaseModel, result, submissionResult);
				testRuns.Add(testRun);
			}
			catch (ExecutionRuntimeException ex)
			{
				testRuns.Add(new TestRunModel()
				{
					TCR_ActualResult = string.Empty,
					TCR_Exception = ex,
					TCR_Status = TestCaseStatus.Error,
					TCR_Parent = testCaseModel,
					TCR_SubmissionResult = submissionResult,
                });
			}
		}

		var status = testRuns.Any(x => x.TCR_Status != TestCaseStatus.Success)
			? testRuns.Any(x => x.TCR_Status == TestCaseStatus.Error) ? ExecutionStatus.Error : ExecutionStatus.Failure
			: ExecutionStatus.Success;

		submissionResult.EXR_TestRuns = testRuns;
		submissionResult.EXR_Status = status;
		return submissionResult;
	}

	object[] GetTestCaseArguments(TestCaseModel testCase)
	{
		var scaffoldParameters = scaffold.EXE_Parameters.ToList();

		if (scaffoldParameters.Count != testCase.TST_Arguments.Count)
		{
			throw new InvalidOperationException("MissMatch between expected number of arguments from scaffold and actual number in test case");
		}

		List<object> arguments = [];
		for (int i = 0; i < testCase.TST_Arguments.Count; i++)
		{
			var targetType = scaffoldParameters[i];
			var targetObject = testCase.TST_Arguments[i];
			var converted = TypeConverter.Convert(targetObject, targetType.TC_Type.OT_Type ?? throw new InvalidOperationException("Parameter type could not be resolved"));
			arguments.Add(converted);
		}

		return arguments.ToArray();
	}

	public virtual TestRunModel AssertTestCase(TestCaseModel testCase, object? actualResult, SubmissionResultModel submissionResult)
	{
		var expectedType = scaffold.EXE_ReturnType?.OT_Type;
		if (expectedType == null)
		{
			return new TestRunModel()
			{
				TCR_ActualResult = string.Empty,
				TCR_Status = TestCaseStatus.Error,
				TCR_Parent = testCase,
				TCR_SubmissionResult = submissionResult,
				TCR_Exception = new ExecutionRuntimeException($"Null return type for test case {testCase.TST_Title}"),
            };
		}

		var expectedResultString = testCase.TST_ExpectedResponse;
		var expectedResult = TypeConverter.Convert(expectedResultString, expectedType);

		var equal = actualResult?.Equals(expectedResult)
			?? throw new InvalidOperationException("Returned null result");
		var status = equal ? TestCaseStatus.Success : TestCaseStatus.Failure;

		return new TestRunModel()
		{
			TCR_ActualResult = actualResult.ToString() ?? string.Empty,
			TCR_Status = status,
			TCR_Parent = testCase,
			TCR_SubmissionResult = submissionResult,
        };
    }
}