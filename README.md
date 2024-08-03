
# CodeTogether

Main project structure:
- CodeTogether - Main application
	- FrontEnd/ A blazor 'standalone' webassembly app ([see](https://learn.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-8.0))
	- BackEnd/ Has backend functionality such as the ASP.NET HTTP API and SignalR API, will also build and statically serve frontend files to allow running as a single unit for development and for initial deployments, later the static frontend files will be hosted via a CDN.
	- Shared/ Used by both frontend and backed, things like message objects
- CodeTogether.Runner - Code execution functionality
	- Is initially to be consumed as an assembly reference by the backend but will containerised and hosted seperatly in the future (gRPC?)
- CodeTogether.Common - Shared code between runner and main application
- CodeTogether.Data - The database model
- CodeTogether.Deployment - Scripts for enviroment setup
