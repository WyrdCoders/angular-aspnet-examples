# WorldCities.Server

## Entity Framework Core

This project uses Entity Framework Core 8. To support multiple versions of EF Core, configure the project to use version 8 by performing the following steps.

#### 1. Create a local tool manifest

In the <b>WorldCities.Server</b> project folder, run `dotnet new tool-manifest`.

#### 2. Install EF as a local tool

In the <b>WorldCities.Server</b> project folder, run `dotnet tool install dotnet-ef --version 8.0.22`.

### Adding the initial migration

In the <b>WorldCities.Server</b> project folder, run `dotnet tool run dotnet-ef migrations add "Initial" -o "Data/Migrations"`. The optional `-o` parameter changes the location where the migration code-generated files are created. Otherwise, a root-level `/Migrations/` folder is created and used by default.

### Updating the database

In the <b>WorldCities.Server</b> project folder, run `dotnet dotnet-ef database update`.
