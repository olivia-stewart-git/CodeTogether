using CodeTogether;
using CodeTogether.Components;
using CodeTogether.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
	.AddRazorComponents()
	.AddInteractiveServerComponents();
builder.Services.AddControllers();
builder.Services
	.RegisterServices()
	.AddDbContext<ApplicationDbContext>();

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

app.Run();