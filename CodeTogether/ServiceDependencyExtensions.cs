using CodeTogether.Data.DataAccess;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace CodeTogether;

public static class ServiceDependencyExtensions
{
	public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddTransient<IUnitOfWorkFactory, UnitOfWorkFactory>();
		return serviceCollection;
	}

	public static IServiceCollection ConfigureLogging(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddSingleton<ILoggerManager, LoggerManager>();
		return serviceCollection;
	}
}