﻿// Not sure if this is required but its what is in
// https://github.com/dotnet/blazor-samples/blob/main/8.0/BlazorSignalRApp/BlazorSignalRApp.Client/Program.cs

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

await builder.Build().RunAsync();
