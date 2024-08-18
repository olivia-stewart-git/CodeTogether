using Blazored.LocalStorage;
using CodeTogether.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services
	.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) })
	.AddScoped<UserStateService>()
	.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
