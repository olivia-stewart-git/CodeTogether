using CodeTogether.Client;
using CodeTogether.Data;
using CodeTogether.Data.Seeding;
using CodeTogether.Hubs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.ResponseCompression;

namespace CodeTogether;

public class Program
{
	public static void Main(string[] args)
	{
		SchemaVersionSeeder.CheckSchemaVersion();

		var builder = WebApplication.CreateBuilder(args);

		builder.Services.RegisterServices();
		builder.Services.RegisterRunnerServices();

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

		app.Run();
	}
}