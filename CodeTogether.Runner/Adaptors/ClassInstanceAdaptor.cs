using CodeTogether.Data.Models.Questions;
using System.Reflection;

namespace CodeTogether.Runner.Adaptors;

public class ClassInstanceAdaptor : TestRunnerAdaptor
{
	public ClassInstanceAdaptor(Assembly targetAssembly, ExecutionConfigurationModel executionConfiguration, IEnumerable<TestCaseModel> testCases) 
		: base(targetAssembly, executionConfiguration, testCases)
	{
	}

	public override IEnumerable<Type> InputTypes { get; } = [];

	public override object? GetExecutionResult(object[] testCaseArguments)
    {
	    var runArguments = executionConfiguration.EXE_AdapterArgument.Split("::", StringSplitOptions.RemoveEmptyEntries);
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

		return method.Invoke(instance, testCaseArguments);
    }
}