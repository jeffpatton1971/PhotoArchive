# Frontend Documentation

This section contains architecture and planning documentation for the PhotoArchive frontend.

## Contents

- [Architecture](architecture.md) — stack selection, app structure, routing, API mapping, and implementation guidance

## Purpose

The v1 frontend is a read-only archive browser built on top of the PhotoArchive API. It provides
year/month/day archive navigation, photo browsing, post and gallery viewing, and on-this-day
discovery — without any CRUD, authentication, or admin workflows.

## Scope

This documentation covers the planned React + TypeScript + Vite frontend application that will live
at `apps/PhotoArchive.Web`. It does not include server-side rendering, mobile apps, or admin tooling.
Those are explicitly deferred to a later phase.

## Related documentation

- [Architecture overview](../architecture/README.md)
- [API reference](../api/README.md)
- [Current status](../overview/current-status.md)
