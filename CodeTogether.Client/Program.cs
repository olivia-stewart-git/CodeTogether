// Not sure if this is required but its what is in
// https://github.com/dotnet/blazor-samples/blob/main/8.0/BlazorSignalRApp/BlazorSignalRApp.Client/Program.cs

using Blazored.LocalStorage;
using CodeTogether.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// for some reason ened both this one and the one in the backend?
builder.Services
	.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) })
	.AddScoped<UserStateService>()
	.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
