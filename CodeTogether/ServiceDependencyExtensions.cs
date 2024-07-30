using CodeTogether.Data;
using CodeTogether.Data.DataAccess;
using CodeTogether.Runner.Scaffolds;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace CodeTogether;

public static class ServiceDependencyExtensions
{
	public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddSingleton<IScaffoldLoader, ScaffoldLoader>();

		serviceCollection.AddTransient<IUnitOfWorkFactory, UnitOfWorkFactory>();

        return serviceCollection;
	}

	public static IServiceCollection ConfigureLogging(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddSingleton<ILoggerManager, LoggerManager>();
		return serviceCollection;
	}
}