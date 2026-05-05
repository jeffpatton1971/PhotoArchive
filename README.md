# PhotoArchive

## Current Status

The PhotoArchive project now includes a working backend API and full data import pipeline.

### Features

- ASP.NET Core API
- PostgreSQL database (Docker)
- EF Core migrations
- Photo import from Jekyll `_gallery` markdown
- 8,500+ photos successfully imported
- Idempotent importer (safe to re-run)

### API Endpoints

- `GET /photos`
- `GET /photos/{slug}`
- `GET /on-this-day`
- `GET /on-this-day?month={m}&day={d}`

### Data Model

Each photo includes:

- Slug (unique)
- Title
- TakenAt / Year / Month / Day
- OriginalUrl / ThumbUrl
- Source
- Gallery / PostUrl / PostId / SortIndex
- Raw source metadata (JSON)

### Import

Run importer:

```bash
dotnet run --project src/PhotoArchive.Tools -- import-jekyll-gallery "<path>" --dry-run
dotnet run --project src/PhotoArchive.Tools -- import-jekyll-gallery "<path>"
