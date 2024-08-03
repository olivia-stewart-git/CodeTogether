
# CodeTogether

Main project structure:
- CodeTogether - Application backend
	- Backend functionality such as the ASP.NET API and SignalR API
	- Serving of client during development
- CodeTogether.Client/ - Application frontend
	- A blazor 'standalone' webassembly app ([see](https://learn.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-8.0)) can build to static files for production
- CodeTogether.Client.Integration/ - Shared code between client and main application such as message objects
- CodeTogether.Runner - Code execution functionality
	- Initially to be consumed as an assembly reference by the backend but may be containerised and hosted seperatly in the future (gRPC?)
- CodeTogether.Data - The database model
- CodeTogether.Deployment - Scripts for enviroment and database setup
- CodeTogether.Common - General utilities and helpers

In production, instances of the backend will be hosted in 1 or more azure windows VM's (sticky-ly routed to by an api gateway).
To start, the frontend will be served by the backend but because it is a seperate project it can be statically hosted in a CDN later.


# Setup
Set startup project to CodeTogether.Deployment and run to setup the database, the run CodeTogether to get the main app

If your intellisense is not working while editing .razor files, follow this https://stackoverflow.com/a/77626381