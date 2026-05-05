# PhotoArchive Architecture

## 1. Introduction and Goals

PhotoArchive is an API-first personal photo archive platform.

It stores photo metadata in PostgreSQL, stores image assets in Azure Blob Storage, and exposes APIs for personal website integration, date-based browsing, albums, individual photo pages, “On This Day” views, curated collections, and future book/export workflows.

### Primary Goals

- Provide one durable metadata record per photo.
- Support individual photo pages.
- Support date navigation by year, month, and day.
- Support “On This Day” queries across years.
- Support album/group browsing.
- Support curated collections for future printed books.
- Keep the system independent from any single static site generator.
- Allow a personal website, currently Jekyll-based, to consume the API.

### Non-Goals

- Full public social photo platform.
- Image editing.
- User-generated uploads from the public.
- Replacing Azure Blob Storage as the image source.

---

## 2. Constraints

- API-first design.
- Main API implemented in C# / ASP.NET Core.
- Database: PostgreSQL.
- Image storage: existing Azure Storage Account / Blob Storage.
- Background processing may use Azure Functions.
- Infrastructure is Azure-based.
- Existing personal website should be able to integrate with the API.
- Long-term archive ownership and portability matter.

---

## 3. Context and Scope

### Users / Consumers

- Personal website visitor
- Site owner / admin
- Future book/export workflow
- Import tools
- Future frontend photo app

### External Systems

- Azure Blob Storage
- PostgreSQL
- Azure App Service
- Azure Functions
- Azure Key Vault
- Application Insights
- Future print/binding service

### System Context

```text
Website / Frontend
        |
        v
PhotoArchive.Api
        |
        +--> PostgreSQL metadata
        |
        +--> Azure Blob Storage image URLs
        |
        +--> Azure Queue / Functions for background jobs
````

---

## 4. Solution Strategy

Use ASP.NET Core Web API as the primary HTTP API.

Use PostgreSQL for structured and semi-structured metadata.

Use Azure Blob Storage for original images, thumbnails, print-ready images, and generated export packages.

Use Azure Functions later for background jobs such as imports, thumbnail generation, metadata extraction, duplicate detection, and book export ZIP generation.

Keep Jekyll or any future website as a consumer of the API, not the source of truth for the archive.

---

## 5. Building Block View

### PhotoArchive.Api

ASP.NET Core API.

Responsibilities:

- Query photos
- Serve photo metadata
- Serve albums
- Serve date-based views
- Serve On This Day results
- Manage collections
- Start export jobs

### PhotoArchive.Core

Domain layer.

Contains:

- Entities
- DTOs
- Interfaces
- Business rules
- Query models

### PhotoArchive.Data

Database access layer.

Contains:

- EF Core DbContext
- Entity configurations
- Migrations
- Repository/query implementations

### PhotoArchive.Functions

Background/event-driven processing.

Future responsibilities:

- Process new images
- Generate thumbnails
- Extract EXIF metadata
- Build book/export ZIP files
- Run scheduled maintenance

### PostgreSQL

Stores:

- Photos
- Albums
- Tags
- Collections
- Export jobs
- EXIF/source metadata as JSONB

### Azure Blob Storage

Stores:

- Original images
- Thumbnails
- Print-ready images
- Export ZIPs
- Import artifacts

---

## 6. Runtime View

### View photos for a date

```text
User opens /photos/2026/05/01
Frontend calls GET /years/2026/months/5/days/1/photos or GET /photos?year=2026&month=5&day=1
API queries PostgreSQL
API returns PagedResponse<PhotoDto> with navigation links and Blob URLs
Frontend renders thumbnails
Browser loads images from Azure Blob Storage
```

### On This Day

```text
User opens /on-this-day/05/01
Frontend calls GET /on-this-day/05/01
API queries photos where month = 5 and day = 1
API returns results across all years
Frontend renders grouped results
```

### Individual photo page

```text
User opens /photos/{photoId}
Frontend calls GET /photos/{photoId}
API returns metadata
Frontend renders image, caption, date, album, tags
Frontend calls related endpoint for same taken_date
```

### Future book export

```text
User creates collection
User adds photos
User requests export
API creates export job
Queue message is created
Azure Function builds ZIP/manifest/captions
Export record updated with Blob URL
```

---

## 7. Deployment View

Azure resources:

- Resource Group
- Azure App Service Plan
- App Service: PhotoArchive.Api
- Azure Database for PostgreSQL Flexible Server
- Azure Storage Account
- Azure Function App
- Azure Key Vault
- Application Insights
- Storage Queue
- Blob containers for originals, thumbs, print, exports

Deployment flow:

```text
GitHub Actions
  -> build/test
  -> apply database migrations
  -> deploy API to App Service
  -> deploy Functions when ready
```

---

## 8. Crosscutting Concepts

### Configuration

Use appsettings locally and Azure App Settings in production.

Secrets stored in Azure Key Vault.

### Identity

Use Managed Identity for Azure resources.

Admin functionality will require authentication later.

### Observability

Use Application Insights for:

- API requests
- errors
- dependency calls
- background job failures

### Metadata Strategy

Core fields are relational.

Variable source metadata goes into JSONB.

Examples:

- Facebook export metadata
- WordPress attachment metadata
- Instagram metadata
- EXIF data

### URL Strategy

Photos have stable IDs and slugs.

Images are served directly from Azure Blob Storage or CDN-backed Blob URLs.

API returns canonical metadata and asset URLs.

### Visibility

Photos should eventually support visibility levels:

- private
- family
- public
- hidden

---

## 9. Architecture Decisions

Architecture decisions are stored in `docs/architecture/adr/`.

Initial ADRs:

- 0001-use-arc42.md
- 0002-api-first-architecture.md
- 0003-use-aspnet-core-for-main-api.md
- 0004-use-postgresql-for-metadata.md
- 0005-use-azure-blob-storage-for-images.md
- 0006-use-azure-functions-for-background-processing.md

### Consistent Photo Collection Pagination

All primary photo collection endpoints return `PagedResponse<PhotoDto>` with `page`, `pageSize`, `totalCount`, `totalPages`, `items`, and pagination `links`.

This applies to:

- `GET /photos`
- `GET /years/{year}/photos`
- `GET /years/{year}/months/{month}/photos`
- `GET /years/{year}/months/{month}/days/{day}/photos`

Pagination is centralized in `PhotoService.GetPagedPhotosAsync(...)`. Flexible query routes such as `/photos?year=&month=&day=` and archive routes such as `/years/{year}/months/{month}/photos` share the same paging, ordering, DTO mapping, and link generation behavior.

---

## 10. Quality Requirements

### Performance

- Date queries should be fast.
- On This Day should return quickly.
- Individual photo metadata lookup should be fast.
- API should support paging for large result sets.

### Maintainability

- C# codebase should remain familiar and testable.
- API, Core, Data, and Functions are separate projects.
- Business logic should not live directly in controllers.

### Scalability

- System should support tens of thousands of photos.
- API should avoid loading the entire archive unnecessarily.
- Background jobs should handle expensive processing.

### Portability

- Metadata should remain exportable.
- Blob URLs and source metadata should be preserved.
- Avoid locking archive meaning into a static site generator.

### Reliability

- Import jobs should be repeatable.
- Duplicate detection should prevent accidental duplication.
- Export jobs should be resumable or retryable.

---

## 11. Risks and Technical Debt

- Metadata quality varies by source.
- Duplicate detection may be difficult.
- Public/private visibility rules are not yet fully defined.
- Book export requirements are not fully known.
- Migration from current Markdown/Jekyll gallery model needs planning.
- Blob URL strategy may change if CDN or signed URLs are introduced.

---

## 12. Glossary

Photo
: A single image or media item.

Album
: A source grouping or logical grouping of photos.

Collection
: A user-curated group of photos, often for export or book creation.

On This Day
: A query showing all photos taken on the same month/day across all years.

Original
: Full-size source image stored in Blob Storage.

Thumbnail
: Smaller image optimized for gallery display.

Print Image
: Image suitable for export or book printing.

Source Metadata
: Original metadata from Facebook, Instagram, WordPress, EXIF, or other imports.
