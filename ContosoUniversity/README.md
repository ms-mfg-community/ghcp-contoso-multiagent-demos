# Contoso University (.NET)

This repository contains a Contoso University sample app implemented as an ASP.NET Core MVC web application with a layered structure (Core/Infrastructure/Web) and an accompanying test suite.

This repo is also used as a teaching/testbed environment. Some items (including at least one unit test) may be intentionally broken to support demonstrations of debugging and GitHub Copilot workflows.

## Quickstart

```powershell
dotnet restore .\ContosoUniversity.sln
dotnet build .\ContosoUniversity.sln -c Debug
dotnet run --project .\ContosoUniversity.Web\ContosoUniversity.Web.csproj
```

In a separate terminal (or after stopping the app):

```powershell
dotnet test .\ContosoUniversity.sln -c Debug
```

## Solution Structure

- `ContosoUniversity.Web` – ASP.NET Core MVC web app (startup project)
- `ContosoUniversity.Core` – domain models + interfaces
- `ContosoUniversity.Infrastructure` – data access + infrastructure services
- `ContosoUniversity.Tests` – xUnit test project

## Prerequisites

- Windows (recommended)
- .NET SDK 8.0+ (the solution targets `net8.0`)
- SQL Server LocalDB (for the default local connection string)

Optional:
- Trust the ASP.NET Core dev HTTPS certificate (recommended for `https://localhost`)

## Build

From the repo root:

```powershell
dotnet restore .\ContosoUniversity.sln
dotnet build .\ContosoUniversity.sln -c Debug
```

## Run Locally

The startup project is the web app:

```powershell
dotnet run --project .\ContosoUniversity.Web\ContosoUniversity.Web.csproj
```

On first run you should see Kestrel listening on one HTTP and one HTTPS URL (ports are defined in the launch profile).

### HTTPS dev cert (optional but recommended)

If you see an HTTPS dev cert warning, run:

```powershell
dotnet dev-certs https --trust
```

## Configuration

- Local connection string: `ConnectionStrings:DefaultConnection` in [ContosoUniversity.Web/appsettings.json](ContosoUniversity.Web/appsettings.json)
- Azure-related settings in `appsettings.json` are placeholders for real deployments.

## Run Tests

Run the full test suite:

```powershell
dotnet test .\ContosoUniversity.sln -c Debug
```

### Note about intentionally failing tests

This repository is used for training scenarios, so you may see one or more tests fail by design (for example, a controller unit test around student pagination). That is expected in this repo.

If you want to run everything except the known failing test(s), you can temporarily filter them out:

```powershell
dotnet test .\ContosoUniversity.sln -c Debug --filter FullyQualifiedName!~StudentsControllerTests.Index_ReturnsViewWithPaginatedList
```

## Features

- Student/Course/Instructor/Department CRUD
- Pagination + search
- Notification system (see [SETUP_TESTING_GUIDE.md](SETUP_TESTING_GUIDE.md) for details)
