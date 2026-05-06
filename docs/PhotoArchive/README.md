# PhotoArchive Documentation

This folder is the canonical documentation home for the PhotoArchive project.

## Sections

- [Overview](overview/current-status.md)
- [Architecture](architecture/api-design.md)
- [Frontend](frontend/README.md)
- [API](api/photos.md)
- [Workflows](workflows/import-jekyll-gallery.md)
- [Components](components/PhotoArchive.Api.md)
- [Decisions](decisions/architecture-decisions.md)
- [Glossary](glossary/glossary.md)
- [Generated reference documentation](generated/README.md)

## Current milestone

PhotoArchive currently has a working API-first backend with PostgreSQL metadata storage, Azure Blob image URLs, a Jekyll gallery importer, DTO-shaped API responses, linked resource responses, archive navigation, and consistent pagination across primary photo collection endpoints.

## Documentation convention

Human-authored documentation lives directly under `docs/PhotoArchive`. Generated code reference documentation should live under `docs/PhotoArchive/generated/<project-name>`.
