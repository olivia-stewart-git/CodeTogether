using CodeTogether.Common.Logging;
using CodeTogether.Data.Models.Factories;
using CodeTogether.Data.Seeding;
using CodeTogether.Runner.Adaptors;
using CodeTogether.Runner.Engine;
using CodeTogether.Runner.Scaffolds;

namespace CodeTogether;

public static class ServiceDependencyExtensions
{
	public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddSingleton<IScaffoldLoader, ScaffoldLoader>();
		serviceCollection.AddSingleton<ICachedTypeModelFactory, ICachedTypeModelFactory>();
		serviceCollection.AddTransient<ISeeder, Seeder>();

        serviceCollection.AddTransient<IExecutionEngine, ExecutionEngine>();
		serviceCollection.AddTransient<ICompilationEngine, CompilationEngine>();

		serviceCollection.AddTransient<IExecutorFactory, ExecutorFactory>();

		return serviceCollection;
	}

	public static IServiceCollection ConfigureLogging(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddSingleton<ILoggerManager, LoggerManager>();
		return serviceCollection;
	}
}