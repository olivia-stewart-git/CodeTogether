using System.Reflection;
using CodeTogether.Data.Models.Questions;
using CodeTogether.Runner.Engine;
using TestCaseStatus = CodeTogether.Data.Models.Questions.TestCaseStatus;

namespace CodeTogether.Runner.Adaptors;

public abstract class TestRunnerAdaptor : IAdaptor
{
	protected readonly Assembly targetAssembly;
	protected readonly ExecutionConfigurationModel executionConfiguration;
	readonly IEnumerable<TestCaseModel> testCases;

	protected TestRunnerAdaptor(Assembly targetAssembly, ExecutionConfigurationModel executionConfiguration, IEnumerable<TestCaseModel> testCases)
	{
		this.targetAssembly = targetAssembly;
		this.executionConfiguration = executionConfiguration;
		this.testCases = testCases;
	}

	public IEnumerable<Type> GetAddTypes() => InputTypes;
    public abstract IEnumerable<Type> InputTypes { get; }

	public abstract object? GetExecutionResult(object[] testCaseArguments);

	public ExecutionResult Execute()
	{
		List<TestRunModel> testRuns = [];
		foreach (var testCaseModel in testCases)
		{
			try
			{
				var arguments = GetTestCaseArguments(testCaseModel);
				var result = GetExecutionResult(arguments);
				var testRun = AssertTestCase(testCaseModel, result);
				testRuns.Add(testRun);
			}
			catch (Exception ex)
			{
				testRuns.Add(new TestRunModel()
				{
					TCR_ActualResult = string.Empty,
					TCR_Exception = ex.ToString(),
					TCR_Status = TestCaseStatus.Error,
					TCR_Parent = testCaseModel,
				});
			}
		}

		var status = testRuns.Any(x => x.TCR_Status != TestCaseStatus.Success) 
			? ExecutionStatus.Failure 
			: ExecutionStatus.Success;

		return new ExecutionResult(status, testRuns);
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

	public virtual TestRunModel AssertTestCase(TestCaseModel testCase, object? inputResult)
	{
		var expectedType = executionConfiguration.EXE_ReturnArgument?.OT_Type;
		if (expectedType == null)
		{
			return new TestRunModel()
			{
				TCR_ActualResult = string.Empty,
				TCR_Status = TestCaseStatus.Error,
				TCR_Parent = testCase,
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
		};
    }
}