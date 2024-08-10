using CodeTogether.Client;
using CodeTogether.Hubs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.ResponseCompression;

namespace CodeTogether;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Services.RegisterServices();
		builder.Services.RegisterRunnerServices();

		//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
		var baseUrl = builder.Configuration.GetValue<string>("BackendUrl") ?? throw new ArgumentNullException();
		builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseUrl) });


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
		//.AddAdditionalAssemblies(typeof(CodeTogether.Client._Imports).Assembly);

		app.UseHttpsRedirection();

		app.UseAuthorization();
		app.UseAuthentication();

		app.UseAntiforgery();
		app.UseStaticFiles();

		app.MapControllers();
		app.MapHub<GameHub>("/gamehub");

		app.Run();
	}
}