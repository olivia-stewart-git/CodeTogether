using CodeTogether.Client;
using CodeTogether.Data;
using CodeTogether.Data.Seeding;
using CodeTogether.Hubs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

namespace CodeTogether;

public class Program
{
	private static WebApplicationBuilder CreateHostBuilder(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Services.RegisterServices();
		builder.Services.RegisterRunnerServices();
		var connectionString = ApplicationDbContext.GetConnectionStringFromConfig(builder.Environment);
		builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

		var baseUrl = builder.Configuration.GetValue<string>("BackendUrl") ?? throw new ArgumentNullException(null);

		builder.Services.AddControllersWithViews().AddNewtonsoftJson();
		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();


		builder.Services.AddSignalR();
		builder.Services.AddRazorComponents().AddInteractiveWebAssemblyComponents();
		builder.Services.AddResponseCompression(opts =>
		{
			opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
				["application/octet-stream"]);
		});

		builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
			.AddCookie(options =>
			{
				options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
				options.SlidingExpiration = true;
				options.AccessDeniedPath = "/Forbidden/";
			});

		builder.WebHost.UseStaticWebAssets();

		return builder;
	}

	public static void Main(string[] args)
	{
		var builder = CreateHostBuilder(args);

		var app = builder.Build();

		var cookiePolicyOptions = new CookiePolicyOptions
		{
			MinimumSameSitePolicy = SameSiteMode.Strict,
		};
		app.UseCookiePolicy(cookiePolicyOptions);

		app.UseResponseCompression();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();

			app.UseWebAssemblyDebugging();
		}

		app.MapRazorComponents<App>()
			.AddInteractiveWebAssemblyRenderMode();

		app.UseHttpsRedirection();

		app.UseAuthorization();
		app.UseAuthentication();

		app.UseAntiforgery();
		app.UseStaticFiles();

		app.MapControllers();
		app.MapHub<LobbyHub>("/api/lobby-hub");
		app.MapHub<GameHub>("/gamehub");

		CheckSchemaVersion(app);
		app.Run();
	}

	static void CheckSchemaVersion(WebApplication app){
		Console.WriteLine($"Env {app.Environment.EnvironmentName}, is_dev={app.Environment.IsDevelopment()}");
		using var scope = app.Services.CreateScope();
		var versionChecker = scope.ServiceProvider.GetRequiredService<SchemaVersionSeeder>();
		versionChecker.CheckSchemaVersion(fixIfOutdated: app.Environment.IsDevelopment());
	}
}