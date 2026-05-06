# PhotoArchive Developer Setup

This guide assumes a clean machine with only VS Code installed.

## Required Tools

### 1. .NET SDK

PhotoArchive uses .NET 10.

Install the latest .NET 10 SDK:

```powershell
winget install Microsoft.DotNet.SDK.10
```

Verify:

```powershell
dotnet --version
```

---

### 2. Node.js LTS

Required for the React + TypeScript + Vite frontend in `apps/PhotoArchive.Web`.

Download and install the current Node.js LTS release:

```powershell
winget install OpenJS.NodeJS.LTS
```

Verify:

```powershell
node --version
npm --version
```

---

### 3. Git

Required for cloning and branch/PR workflow.

```powershell
winget install Git.Git
```

Verify:

```powershell
git --version
```

---

### 4. Docker Desktop

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

### 5. PostgreSQL Client Tools

Optional but useful for inspecting the database.

```powershell
winget install PostgreSQL.PostgreSQL.16
```

Or use a GUI like:

```powershell
winget install dbeaver.dbeaver
```

---

### 6. VS Code Extensions

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

Restore and build the .NET projects:

```powershell
dotnet restore
dotnet build
```

Install and build the frontend:

```powershell
npm install --prefix apps/PhotoArchive.Web
npm run build --prefix apps/PhotoArchive.Web
```

---

## Database Setup

Start PostgreSQL using Docker Desktop.

Create and start the local PostgreSQL container:

```powershell
docker run --name photoarchive-postgres `
  -e POSTGRES_DB=photoarchive `
  -e POSTGRES_USER=postgres `
  -e POSTGRES_PASSWORD=postgres `
  -p 5432:5432 `
  -d postgres:16
```

If the container already exists but is stopped:

```powershell
docker start photoarchive-postgres
```

This project stores secrets in the [Secret Manager](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/secure-net-microservices-web-applications/developer-app-secrets-storage) so this needs to be configured.

```powershell
dotnet user-secrets set "ConnectionStrings:Default" "Host=localhost;Port=5432;Database=photoarchive;Username=postgres;Password=postgres" --project src/PhotoArchive.Api

dotnet user-secrets set "ConnectionStrings:Default" "Host=localhost;Port=5432;Database=photoarchive;Username=postgres;Password=postgres" --project src/PhotoArchive.Tools
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

## Run the Frontend

```powershell
npm run dev --prefix apps/PhotoArchive.Web
```

Vite:

```text
http://localhost:5173
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
apps/PhotoArchive.Web       React + TypeScript + Vite frontend
tests/PhotoArchive.Tests    API and service tests
docs/PhotoArchive           Project documentation
```

---

## Minimum Setup Checklist

```text
[ ] .NET 10 SDK
[ ] Node.js LTS
[ ] Git
[ ] Docker Desktop
[ ] VS Code extensions
[ ] dotnet restore
[ ] dotnet build
[ ] npm install --prefix apps/PhotoArchive.Web
[ ] npm run build --prefix apps/PhotoArchive.Web
[ ] start photoarchive-postgres container
[ ] dotnet ef database update
[ ] dotnet run --project src/PhotoArchive.Api
[ ] npm run dev --prefix apps/PhotoArchive.Web
[ ] open /swagger
[ ] dotnet test
```
