using CodeTogether.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Identity.Abstractions;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;

namespace CodeTogether
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllers();
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

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();

				// https://github.com/dotnet/blazor-samples/blob/main/8.0/BlazorSignalRApp/BlazorSignalRApp/Program.cs
				var razorBuilder = app.MapRazorComponents<App>();
				//razorBuilder.AddInteractiveWebAssemblyRenderMode();
				//var assembly = typeof(CodeTogether.Client._Imports).Assembly;
				//razorBuilder.AddAdditionalAssemblies(assembly);
				// app.UseWebAssemblyDebugging();
			}

			app.UseHttpsRedirection();
			//app.UseAuthorization();

			app.UseStaticFiles();
			app.UseAntiforgery();

			app.MapControllers();
			// app.MapHub<GameHub>("/gamehub");

			app.Run();
		}
	}
}
