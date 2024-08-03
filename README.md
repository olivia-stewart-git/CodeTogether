
# CodeTogether

Main project structure:
- CodeTogether - Main application backend
	- Backend functionality such as the ASP.NET API and SignalR API
- CodeTogether.Client/ - Application frontend
	- A blazor 'standalone' webassembly app ([see](https://learn.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-8.0)) which can host itself during development or build to static files for production
- CodeTogether.Client.Integration/ - Shared code between client and main application such as message objects
- CodeTogether.Runner - Code execution functionality
	- Initially to be consumed as an assembly reference by the backend but will be containerised and hosted seperatly in the future (gRPC?)
- CodeTogether.Data - The database model
- CodeTogether.Deployment - Scripts for enviroment and database setup
- CodeTogether.Common - General utilities and helpers
- CodeTogether.App.Test - Tests for CodeTogether main backend and services

During development multiple startup targets will be used to coordinate running backend and frontend at the same time.
In production instances of the backend will be hosted in 1 or more azure windows VM's (sticky-ly routed to by an api gateway), the frontend is built to static files and will be hosted with azure static file hosting.

The decision to have the frontend and backend in seperate projects and deployed seperatly was made beacuse it creates a better seperation of concerns between UI and business logic. Blazor requires the use of a seperate project for webassembly components