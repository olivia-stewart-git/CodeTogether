
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

When adding new blazor or css files make sure to set their build action property as Content rather than the default of None


# Deployment

The deployment is a manual process as follows:
- Download build artifacts from the most recent build action and scp them to the app server (e.g. `scp -i ~/azure_vms.pem build-artifact.zip azureuser@{IP ADDRESS}:~/build-artifact.zip`)
- Kill existing process (``) and unzip binaries
- Update appsettings.json with the sql server connection string, a copy of the prod db appsettings should be kept seperate from the source code so it can just be copied.
- Kill existing process and then run `./CodeTogether.Deployment Seedonly` to migrate and seed the database
- Run application in background with `nohup ./CodeTogether > ~/logs/out.log 2> ~/logs/err.log &`

The azure database does not allowing dropping the database because it is managed from the portal so the database schema changes will be managed with normal ef core migrations.
The migrations will be applied with the CodeTogether.Deployment executable (`./CodeTogether SeedOnly`) along with seeding, or updating the seeded data.
