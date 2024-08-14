using CodeTogether.Data.Models.Questions;
using CodeTogether.Runner.Engine;
using System.Reflection;
using TestCaseStatus = CodeTogether.Data.Models.Questions.TestCaseStatus;

namespace CodeTogether.Runner.Adaptors;

public abstract class TestRunnerSubmissionExecutor : ISubmissionExecutor
{
	protected readonly ExecutionConfigurationModel executionConfiguration;
	readonly IEnumerable<TestCaseModel> testCases;

	protected TestRunnerSubmissionExecutor(ExecutionConfigurationModel executionConfiguration, IEnumerable<TestCaseModel> testCases)
	{
		this.executionConfiguration = executionConfiguration;
		this.testCases = testCases;
	}

	public IEnumerable<Type> GetAddTypes() => InputTypes;
	public abstract IEnumerable<Type> InputTypes { get; }

	public abstract object? GetExecutionResult(Assembly targetAssembly, object[] testCaseArguments);

	public ExecutionResultModel Execute(Assembly targetAssembly)
	{
		TestRunExecutionModel fullExecution = new TestRunExecutionModel();
		List<TestRunModel> testRuns = [];
		foreach (var testCaseModel in testCases)
		{
			try
			{
				var arguments = GetTestCaseArguments(testCaseModel);
				var result = GetExecutionResult(targetAssembly, arguments);
				var testRun = AssertTestCase(testCaseModel, result, fullExecution);
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
					TCT_Execution = fullExecution,
                });
			}
		}

		var status = testRuns.Any(x => x.TCR_Status != TestCaseStatus.Success)
			? testRuns.Any(x => x.TCR_Status == TestCaseStatus.Error) ? ExecutionStatus.Error : ExecutionStatus.Failure
			: ExecutionStatus.Success;

		fullExecution.TRX_TestRuns = testRuns;
		return new ExecutionResultModel
		{
			EXR_Status = status,
			EXR_TestRun = fullExecution,
        };
	}

	object[] GetTestCaseArguments(TestCaseModel testCase)
	{
		var inputTypes = executionConfiguration.EXE_InputArguments?.TC_Types;
		if (inputTypes == null || inputTypes.Count == 0)
		{
			return [];
		}

		if (inputTypes.Count != testCase.TST_Arguments.Length)
		{
			throw new InvalidOperationException("MissMatch in test case arguments");
		}

		List<object> objects = [];
		for (int i = 0; i < testCase.TST_Arguments.Length; i++)
		{
			var targetType = inputTypes[i];
			var targetObject = testCase.TST_Arguments[i];
			var converted = TypeConverter.Convert(targetObject, targetType.OT_Type ?? throw new InvalidOperationException("Cannot expect null argument"));
			objects.Add(converted);
		}

		return objects.ToArray();
	}

	public virtual TestRunModel AssertTestCase(TestCaseModel testCase, object? inputResult, TestRunExecutionModel testRun)
	{
		var expectedType = executionConfiguration.EXE_ReturnArgument?.OT_Type;
		if (expectedType == null)
		{
			return new TestRunModel()
			{
				TCR_ActualResult = string.Empty,
				TCR_Status = TestCaseStatus.Error,
				TCR_Parent = testCase,
				TCT_Execution = testRun,
				TCR_Exception = new ExecutionRuntimeException($"Null return type for test case {testCase.TST_Title}"),
            };
		}

		var expectedResult = testCase.TST_ExpectedResponse;
		if (expectedResult == "null" && inputResult is null)
		{
			return new TestRunModel()
			{
				TCR_ActualResult = "null",
				TCR_Status = TestCaseStatus.Success,
				TCR_Parent = testCase,
				TCT_Execution = testRun,
            };
        }
		var convertExpected = TypeConverter.Convert(expectedResult, expectedType);

		var equal = inputResult?.Equals(convertExpected)
			?? throw new InvalidOperationException("Returned null result");
		var status = equal ? TestCaseStatus.Success : TestCaseStatus.Failure;

		return new TestRunModel()
		{
			TCR_ActualResult = inputResult.ToString() ?? string.Empty,
			TCR_Status = status,
			TCR_Parent = testCase,
			TCT_Execution = testRun,
        };
    }
}