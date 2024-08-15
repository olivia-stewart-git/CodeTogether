using CodeTogether.Common.Logging;
using CodeTogether.Data;
using CodeTogether.Data.Seeding;
using CodeTogether.Runner.Adaptors;
using CodeTogether.Runner.Engine;
using CodeTogether.Service.Games;
using CodeTogether.Services.Authentication;
using CodeTogether.Services.Games;

namespace CodeTogether;

public static class ServiceDependencyExtensions
{
	public static IServiceCollection RegisterRunnerServices(this IServiceCollection services)
	{
		services.AddTransient<IExecutionEngine, ExecutionEngine>();
		services.AddTransient<ICompilationEngine, CompilationEngine>();
		services.AddTransient<IExecutorFactory, ExecutorFactory>();

		return services;
	}

	public static IServiceCollection RegisterServices(this IServiceCollection services)
	{
		services.AddDbContext<ApplicationDbContext>();
		services.AddTransient<SchemaVersionSeeder>();
		services.AddTransient<ILobbyService, LobbyService>();
		services.AddTransient<IUserService, UserService>();
		services.AddSingleton<IGameService, GameService>();

		services.AddTransient<ICryptographyService, CryptographyService>();
		services.AddTransient<IRegistrationService, RegistrationService>();
		services.AddTransient<IRegisterVerificationService, RegisterVerificationService>();
		services.AddTransient<ILoginAuthenticationService, LoginAuthenticationService>();

		return services;
	}

	public static IServiceCollection ConfigureLogging(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddSingleton<ILoggerManager, LoggerManager>();
		return serviceCollection;
	}
}