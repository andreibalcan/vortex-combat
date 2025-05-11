# Vortex Combat Server
This file contains information about the back-end of **Vortex Combat**. This server is responsible for managing authentication, user roles, and all core logic.

## 🧩 Architecture
This server follows a standard RESTful architecture to expose resources via HTTP, and uses:
- **Entity Framework** Core for ORM (Object-Relational Mapping).
- **ASP.NET Core Identity** for user management.
- **JWT (JSON Web Tokens)** for authentication.

### Clean Architecture
This server follows the **Clean Architecture** (also known as Onion Architecture) principles, which promote separation of concerns, testability, and scalability.

<div align="center">
  <img src="https://miro.medium.com/v2/resize:fit:500/1*sura91gPMoCjPNvZWsAO_g.png" width="300" alt="Clean Architecture Diagram" />
</div>

The solution is organized into distinct layers:
```bash
server/
├── VortexCombat.Domain         // Core business models & logic (Entities)
├── VortexCombat.Application    // Use cases & DTOs (Application layer)
├── VortexCombat.Infrastructure // Implementation details (EF Core, Services)
├── VortexCombat.Presentation   // API layer (Controllers, Startup)
├── VortexCombat.Shared         // Shared enums, constants, and contracts
├── VortexCombat.sln            // Solution file
```

## ⚙ Requirements
Before running this project, make sure your system is set up with the required tooling.

### .NET 9 SDK
This project uses **.NET 9.0.0** (the latest version at the time of writing).

> Download .NET 9 [here](https://dotnet.microsoft.com/en-us/download/dotnet/9.0).

### MySQL
This project uses **MySQL 8.4.4** as the databse.

> Download MySQL 8.4.4 [here](https://dev.mysql.com/doc/relnotes/mysql/8.4/en/news-8-4-4.html).

### Environment Variables
You must configure a `.env` file at the root of the project. Use `.env-example` as a template:

```
cp .env-example .env
```

Then fill in the required fields, which include:
- Databse connection string.
- JWT settings.
- Default admin credentials.

## 🛠 First-Time Setup
Follow these steps to set up the project locally:

#### 1. Restore NuGet packages
Make sure all required dependencies listed in the `.csproj` are downloaded.

```
dotnet restore
```
In some ocasions you might need to manually instal the **dotnet-ef** package manually.

You can do it via:
`dotnet tool install --global dotnet-ef --version 9.0.3` then _follow recommended zsh steps to fix the path_.

#### 2. Create the initial Database Migration
Generate the schema snapshot that EF Core will use to create tables based on our entity classes.

Navigate to the Presentation Layer:
```
cd VortexCombat.Presentation
```

```
dotnet ef migrations add InitialCreate \
  --startup-project ../VortexCombat.Presentation \
  --project ../VortexCombat.Infrastructure
```

```
dotnet ef database update \
  --startup-project ../VortexCombat.Presentation \
  --project ../VortexCombat.Infrastructure
```

In case you want to remove your existing migrations and start from scratch, run:
```
dotnet ef migrations remove
```

#### 3. Running the app
Once your environment is set up and the database is ready, you can build and run the server (inside the Presentation layer):

```
dotnet build
dotnet run
```

You can then explore and test all available endpoints through Swagger, available at:
> https://localhost:5299/swagger
