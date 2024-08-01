using CodeTogether.Data.Models.Questions;
using System.Reflection;
using CodeTogether.Runner.Engine;

namespace CodeTogether.Runner.Adaptors;

public class ClassInstanceSubmissionExecutor : TestRunnerSubmissionExecutor
{
	public ClassInstanceSubmissionExecutor(ExecutionConfigurationModel executionConfiguration, IEnumerable<TestCaseModel> testCases) 
		: base(executionConfiguration, testCases)
	{
	}

	public override IEnumerable<Type> InputTypes { get; } = [];

	public override object? GetExecutionResult(Assembly targetAssembly, object[] testCaseArguments)
    {
	    var runArguments = executionConfiguration.EXE_ExecutionArgument.Split("::", StringSplitOptions.RemoveEmptyEntries);
		var typeName = runArguments[0];
		var methodName = runArguments[1];

		var type = targetAssembly.GetType(typeName);
		if (type == null)
		{
			throw new InvalidOperationException($"Could not resolve run type {typeName}");
		}

		var method = type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).FirstOrDefault(x => x.Name == methodName);
		if (method == null)
		{
			throw new InvalidOperationException($"Could not resolve method {methodName}");
		}

		var instance = Activator.CreateInstance(type);

		try
		{
			return method.Invoke(instance, testCaseArguments);
		}
		catch (Exception ex)
		{
			throw new ExecutionRuntimeException("Exception occurred running test", ex);
		}
    }
}