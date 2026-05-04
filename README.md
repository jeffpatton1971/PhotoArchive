# PhotoArchive

## Current State

- ASP.NET Core API running
- PostgreSQL (Docker) connected
- EF Core migrations working
- /photos endpoint implemented
- Local secrets managed via user-secrets
- Add photo service, filtering, and on-this-day endpoint
- Introduce PhotoService for query logic
- Add filtering to GET /photos (year/month/day)
- Add GET /on-this-day endpoint (supports today + query params)
- Wire up EF Core with PostgreSQL (Docker)
- Use user-secrets for local connection string
  