using CodeTogether.Common.Logging;
using CodeTogether.Data.DataAccess;
using CodeTogether.Runner.Engine;
using CodeTogether.Runner.Scaffolds;

namespace CodeTogether;

public static class ServiceDependencyExtensions
{
	public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddSingleton<IScaffoldLoader, ScaffoldLoader>();

		serviceCollection.AddTransient<IExecutionEngine, ExecutionEngine>();
		serviceCollection.AddTransient<ICompilationEngine, CompilationEngine>();

        return serviceCollection;
	}

	public static IServiceCollection ConfigureLogging(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddSingleton<ILoggerManager, LoggerManager>();
		return serviceCollection;
	}
}