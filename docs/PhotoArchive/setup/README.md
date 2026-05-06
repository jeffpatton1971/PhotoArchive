# PhotoArchive Developer Setup

This guide assumes a clean machine with only VS Code installed.

## Required Tools

### 1. .NET SDK

PhotoArchive uses .NET 10.

Install the latest .NET 10 SDK:

```powershell
winget install Microsoft.DotNet.SDK.10
````

Verify:

```powershell
dotnet --version
```

---

### 2. Git

Required for cloning and branch/PR workflow.

```powershell
winget install Git.Git
```

Verify:

```powershell
git --version
```

---

### 3. Docker Desktop

Used for local PostgreSQL.

```powershell
winget install Docker.DockerDesktop
```

After install, start Docker Desktop.

Verify:

```powershell
docker version
```

---

### 4. PostgreSQL Client Tools

Optional but useful for inspecting the database.

```powershell
winget install PostgreSQL.PostgreSQL
```

Or use a GUI like:

```powershell
winget install dbeaver.dbeaver
```

---

### 5. VS Code Extensions

Recommended:

```text
C# Dev Kit
C#
Docker
REST Client
GitHub Pull Requests
PowerShell
Markdown All in One
```

---

## Clone Repository

```powershell
git clone https://github.com/jeffpatton1971/PhotoArchive.git
cd PhotoArchive
```

---

## Restore and Build

```powershell
dotnet restore
dotnet build
```

---

## Database Setup

Start PostgreSQL using Docker/Docker Compose if configured in the repo.

Typical command:

```powershell
docker compose up -d
```

Then apply EF migrations:

```powershell
dotnet ef database update --project src/PhotoArchive.Data --startup-project src/PhotoArchive.Api
```

If `dotnet ef` is missing:

```powershell
dotnet tool install --global dotnet-ef
```

---

## Run the API

```powershell
dotnet run --project src/PhotoArchive.Api
```

Swagger:

```text
http://localhost:5296/swagger
```

---

## Import Data

Dry run first:

```powershell
dotnet run --project src/PhotoArchive.Tools -- import-jekyll-gallery "C:\path\to\site\_gallery" --dry-run
```

Then import:

```powershell
dotnet run --project src/PhotoArchive.Tools -- import-jekyll-gallery "C:\path\to\site\_gallery"
```

---

## Run Tests

```powershell
dotnet test
```

---

## Generate Documentation

Restore local tools:

```powershell
dotnet tool restore
```

Build generated docs:

```powershell
./scripts/build-docs.ps1 -Configuration Release
```

Generated files land under:

```text
docs/PhotoArchive/generated
```

---

## Useful API Smoke Tests

```powershell
Invoke-RestMethod "http://localhost:5296/photos?page=1&pageSize=5" | ConvertTo-Json -Depth 6
Invoke-RestMethod "http://localhost:5296/years" | ConvertTo-Json -Depth 6
Invoke-RestMethod "http://localhost:5296/on-this-day" | ConvertTo-Json -Depth 6
Invoke-RestMethod "http://localhost:5296/photos?source=instagram&page=1&pageSize=5" | ConvertTo-Json -Depth 6
```

---

## Main Project Areas

```text
src/PhotoArchive.Api        REST API
src/PhotoArchive.Core       DTOs, models, link builders
src/PhotoArchive.Data       EF Core, PostgreSQL, services
src/PhotoArchive.Tools      Import tooling
src/PhotoArchive.Functions  Azure Functions experiments
tests/PhotoArchive.Tests    API and service tests
docs/PhotoArchive           Project documentation
```

---

## Minimum Setup Checklist

```text
[ ] .NET 10 SDK
[ ] Git
[ ] Docker Desktop
[ ] VS Code C# extensions
[ ] dotnet restore
[ ] docker compose up -d
[ ] dotnet ef database update
[ ] dotnet run --project src/PhotoArchive.Api
[ ] open /swagger
[ ] dotnet test
