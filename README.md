# PhotoArchive

PhotoArchive is an API-first personal photo archive platform focused on long-term preservation, archive navigation, and rich photo discovery experiences.

The project currently provides:

- A stable ASP.NET Core REST API
- Import tooling for legacy Jekyll photo galleries
- PostgreSQL-backed metadata storage
- Archive navigation by year/month/day
- Hypermedia-style API responses
- Generated OpenAPI/Swagger documentation
- Integration test coverage
- CI-based documentation generation

---

## Project Status

## API Version

The current `main` branch represents the initial stable API surface (`v1` candidate).

The API now includes:

- Stable routes
- Centralized querying
- Consistent pagination
- Integration tests
- Swagger/OpenAPI metadata
- XML-generated reference documentation

---

## Architecture Overview

PhotoArchive is organized into several projects:

| Project | Purpose |
| --- | --- |
| `PhotoArchive.Api` | ASP.NET Core REST API |
| `PhotoArchive.Core` | Shared models, DTOs, link generation |
| `PhotoArchive.Data` | EF Core data access and services |
| `PhotoArchive.Tools` | Import and maintenance tooling |
| `PhotoArchive.Functions` | Azure Functions integration experiments |
| `PhotoArchive.Tests` | Integration and API tests |

Additional documentation lives under:

```text
/docs/PhotoArchive
```

---

## Current Features

## Archive Navigation

Browse photos by archive hierarchy:

```text
/years
/years/{year}
/years/{year}/months/{month}
/years/{year}/months/{month}/days/{day}
```

### Photo Browsing

```text
/photos
/photos/{slug}
```

Supports filtering by:

- year
- month
- day
- source
- gallery
- postId

Example:

```text
/photos?source=instagram&page=1&pageSize=25
```

---

### Posts

```text
/posts/{postId}
/posts/{postId}/photos
```

---

### Galleries

```text
/galleries/{gallery}/photos
```

---

### On This Day

```text
/on-this-day
/on-this-day?month=5&day=5
```

---

## Pagination

Photo collection endpoints use a shared paged response format:

```json
{
  "page": 1,
  "pageSize": 25,
  "totalCount": 352,
  "totalPages": 15,
  "items": [],
  "links": {
    "self": {},
    "first": {},
    "previous": {},
    "next": {},
    "last": {}
  }
}
```

Pagination links preserve active filters.

---

## Data Model

Each imported photo currently includes:

- Slug
- Title
- TakenAt
- Year / Month / Day
- Source
- OriginalUrl
- ThumbUrl
- Gallery
- PostId
- PostUrl
- SortIndex
- RawMetadata

---

## Import Pipeline

PhotoArchive currently imports legacy Jekyll `_gallery` markdown content.

### Dry Run

```bash
dotnet run --project src/PhotoArchive.Tools -- import-jekyll-gallery "<path>" --dry-run
```

### Import

```bash
dotnet run --project src/PhotoArchive.Tools -- import-jekyll-gallery "<path>"
```

The importer is idempotent and safe to re-run.

---

## Development

See the [setup](docs/PhotoArchive/setup/README.md) documentatoin in the docs folder

---

## Documentation

## Architecture & Project Docs

```text
/docs/PhotoArchive
```

## Generated API Docs

Generated from XML comments using `xml2doc`.

Regenerate locally:

```powershell
./scripts/build-docs.ps1 -Configuration Release
```

---

## CI/CD

The repository includes workflows for:

- build validation
- test execution
- generated documentation
- documentation PR generation

---

## Testing

The repository includes integration tests covering:

- archive navigation
- pagination
- filter preservation
- posts
- galleries
- on-this-day
- photo detail routes
- response consistency

---

## Design Principles

PhotoArchive is intentionally:

- API-first
- archive-oriented
- resource-driven
- queryable
- frontend-agnostic

The API is designed to support future:

- React-based archive experiences
- gallery browsing
- timeline navigation
- social-style photo discovery
- media management workflows

---

## Roadmap

### Current Focus

- API stabilization
- frontend exploration
- archive browsing UX
- documentation quality

### Future Exploration

- React frontend
- authenticated management UI
- CRUD workflows
- first-class gallery entities
- cover photo selection
- richer search/filtering
- tagging/location metadata
- uploads and editing

---

## Repository Documentation

See:

```text
/docs/PhotoArchive
```

for detailed architecture, API, and generated reference documentation.
