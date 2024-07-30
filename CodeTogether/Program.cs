using CodeTogether;
using CodeTogether.Components;
using CodeTogether.Data;
using CodeTogether.Data.Seeding;
using NLog;
using NLog.Web;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

LogManager.Setup().LoadConfiguration(builder =>
{
	builder.ForLogger().FilterMinLevel(NLog.LogLevel.Info).WriteToColoredConsole();
});

var logger = NLog.LogManager.GetCurrentClassLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Host.UseNLog();

// Add services to the container.
builder.Services.ConfigureLogging();
builder.Services
	.AddRazorComponents()
	.AddInteractiveServerComponents();
builder.Services
	.RegisterServices()
	.AddDbContext<ApplicationDbContext>();

builder.Services.AddControllers();

var app = builder.Build();
app.MapControllers();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

using (var context = new ApplicationDbContext())
{
	new Seeder(context).Seed();
}

app.Run();