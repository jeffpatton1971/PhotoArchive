# Frontend Architecture

## 1. Purpose

The v1 frontend is a **read-only archive browser**. Its job is to let users browse photos by
date, view individual photos, read posts, explore galleries, and discover what happened on a
given day in past years.

This phase does **not** include:

- CRUD operations (create, update, delete)
- Authentication or user accounts
- Photo uploads
- Admin or editorial workflows
- Gallery management
- Cover photo selection

Those capabilities are explicitly deferred until a later phase when the product direction is
clearer.

---

## 2. Recommended stack

**React + TypeScript + Vite**

| Technology | What it does | Why it was chosen |
|---|---|---|
| **React** | Builds the UI from reusable components | Widely adopted, large ecosystem, easy to start with |
| **TypeScript** | Adds types to JavaScript | Catches mistakes at edit time; easier to work with API response shapes |
| **Vite** | Development server and build tool | Fast, lightweight, minimal configuration required |

### Plain-language explainer for each piece

**React** is a way of building web pages from small, reusable pieces called components. Think of a
component the way you might think of an ASP include file or a partial template — it is a fragment
of the page that can be reused and given different data each time it is rendered.

**TypeScript** is JavaScript with optional type annotations. If the API returns a photo object
with a `slug` property, TypeScript can warn you if you accidentally try to read `photo.slud`
instead of `photo.slug`. You do not need to learn all of TypeScript upfront — adding types
gradually as you learn is a perfectly fine approach.

**Vite** replaces the older Create React App toolchain. It starts a development server quickly,
reloads the browser when you save a file, and produces an optimised build for deployment. You will
rarely need to touch its configuration.

---

## 3. Why not Jekyll as the primary frontend

Jekyll is already used to generate the gallery content that PhotoArchive imports. It is useful
in that role and may stay useful for static content generation.

However, Jekyll is a static site generator. It builds HTML files at deploy time from templates and
data files. The PhotoArchive API is a live service that returns data on demand, supports
pagination, filtering by source and gallery, and provides archive navigation with variable
date ranges.

A React frontend is a better match because:

- It can call the API at runtime and respond to user interaction (pagination, filtering, browsing)
- It does not require a full rebuild every time the archive changes
- It can share navigation state across pages without a full-page reload
- It composes naturally with the paginated, hierarchical API responses

Jekyll may still be useful for generating static documentation or content, but it is not the right
primary frontend for a dynamic archive browser.

---

## 4. Why not Next.js yet

Next.js is a React framework that adds server-side rendering (SSR), static generation, file-based
routing, and other features on top of React.

Those features are valuable in certain contexts, but they add complexity early on:

- SSR requires a Node.js server to be running alongside the API
- The file-based routing convention is different from plain React Router
- Deployment is more involved than a simple static file host
- The product direction for this archive is not yet settled enough to commit to a server-rendered
  architecture

A plain React + Vite application can be deployed as static files to any CDN or file host. If SSR
or static generation becomes necessary later (for SEO, performance, or other reasons), migrating
to Next.js at that point is straightforward.

---

## 5. Proposed app location

The React application will live at `apps/PhotoArchive.Web`, alongside the existing backend
projects.

```
apps/
  PhotoArchive.Web/         ← React + TypeScript + Vite app (this phase)
src/
  PhotoArchive.Api/         ← ASP.NET Core Web API
  PhotoArchive.Core/        ← Domain models and interfaces
  PhotoArchive.Data/        ← EF Core data access
  PhotoArchive.Tools/       ← Import tooling
tests/
  PhotoArchive.Tests/       ← Integration tests
docs/
  PhotoArchive/             ← Human-authored documentation
```

The `apps/` folder is new. It is intended for client applications that consume the API, keeping
them separate from the server-side source code in `src/`.

---

## 6. Frontend responsibilities

The frontend owns everything the user sees and interacts with in the browser. It is responsible
for:

- **Layout** — the overall page structure, header, navigation, and footer
- **Navigation** — routing between archive pages, photo detail, posts, galleries, and on-this-day
- **Rendering photos** — displaying photo thumbnails in a grid and individual photos in a detail view
- **Calling the API** — fetching data from the PhotoArchive API on demand
- **Managing local UI state** — tracking which page the user is on, whether a photo is loading,
  and similar browser-side concerns
- **Pagination UI** — rendering next/previous page controls and passing page parameters to the API

The frontend does **not** contain business logic. It does not know how photos are stored, how
archive dates are computed, or how galleries are organised. It only knows how to ask the API for
data and how to display the response.

---

## 7. Backend responsibilities

The API handles all data concerns. It is responsible for:

- **Filtering** — by source, gallery, and other criteria
- **Pagination** — returning a page of results with total count and page metadata
- **Archive hierarchy** — organising photos by year, month, and day
- **Post and gallery relationships** — linking photos to posts and galleries
- **On-this-day grouping** — finding photos taken on the same calendar date in previous years
- **Data persistence** — reading from the PostgreSQL database and returning DTO-shaped responses

The API is the source of truth. The frontend never directly accesses the database or performs
business logic calculations.

---

## 8. Initial frontend routes

The following URL routes will be supported in the v1 frontend:

| Route | Description |
|---|---|
| `/` | Home page |
| `/archive` | Archive root — lists available years |
| `/archive/:year` | Year view — lists months with photos in that year |
| `/archive/:year/:month` | Month view — lists days with photos in that month |
| `/archive/:year/:month/:day` | Day view — shows photos taken on that day |
| `/photos/:slug` | Photo detail — shows a single photo |
| `/posts/:postId` | Post detail — shows a post and its photos |
| `/galleries/:gallery` | Gallery view — shows photos in a gallery |
| `/on-this-day` | On-this-day — shows photos from this calendar date in past years |

`:year`, `:month`, `:day`, `:slug`, `:postId`, and `:gallery` are URL parameters that the page
component reads to make its API request.

---

## 9. API mapping

Each frontend route calls one or more API endpoints. The table below shows the mapping.

| Frontend route | API call(s) |
|---|---|
| `/archive` | `GET /years` |
| `/archive/:year` | `GET /years/{year}` |
| `/archive/:year/:month` | `GET /years/{year}/months/{month}` |
| `/archive/:year/:month/:day` | `GET /years/{year}/months/{month}/days/{day}/photos` |
| `/photos/:slug` | `GET /photos/{slug}` |
| `/posts/:postId` | `GET /posts/{postId}` and `GET /posts/{postId}/photos` |
| `/galleries/:gallery` | `GET /galleries/{gallery}/photos` |
| `/on-this-day` | `GET /on-this-day` |

All list endpoints support pagination via `page` and `pageSize` query parameters. The frontend
passes those parameters when the user navigates between pages.

---

## 10. Proposed source structure

The application source lives under `apps/PhotoArchive.Web/src/`.

```
apps/PhotoArchive.Web/
  src/
    api/
      photoArchiveApi.ts        ← All API fetch functions in one place
    components/
      PhotoCard.tsx             ← Single photo thumbnail
      PhotoGrid.tsx             ← Grid of PhotoCard components
      YearCard.tsx              ← Summary card for a year
      MonthCard.tsx             ← Summary card for a month
      DayCard.tsx               ← Summary card for a day
      PaginationLinks.tsx       ← Next / previous page controls
    pages/
      HomePage.tsx              ← Route: /
      ArchivePage.tsx           ← Route: /archive
      YearPage.tsx              ← Route: /archive/:year
      MonthPage.tsx             ← Route: /archive/:year/:month
      DayPage.tsx               ← Route: /archive/:year/:month/:day
      PhotoDetailPage.tsx       ← Route: /photos/:slug
      PostPage.tsx              ← Route: /posts/:postId
      GalleryPage.tsx           ← Route: /galleries/:gallery
      OnThisDayPage.tsx         ← Route: /on-this-day
```

`photoArchiveApi.ts` is the single module that knows the API base URL and how to construct
requests. Page components import functions from it rather than calling `fetch` directly. This
makes it easy to update the API base URL or request headers in one place.

Page components (`pages/`) fetch data and own the route-level state. They pass data down to
display components (`components/`) as props. Display components are stateless — they render
whatever they are given.

---

## 11. React learning model

If you are coming from HTML and ASP-style web development, here is how the core React concepts
map to things you already know.

| React concept | Equivalent in HTML / ASP | Plain-language description |
|---|---|---|
| Component | ASP include file / partial template | A reusable fragment of the page that can be given different data each time |
| Props | Parameters passed to an include | Values passed into a component from the outside; the component cannot change them |
| State | A variable stored in the page's script | Data that the browser remembers while the user is on the page; when it changes, the page re-renders |
| Effect (`useEffect`) | `Page_Load` or an AJAX callback | Code that runs after the component renders; the usual place to call the API |
| Router | URL routing in ASP.NET | Maps browser URLs to React page components; reads URL parameters like `:year` or `:slug` |

A minimal pattern for a page component looks like this:

```tsx
// YearPage.tsx
// 1. Read the URL parameter
// 2. When the component loads, fetch data from the API
// 3. Store the result in state
// 4. Render the result
```

You do not need to learn the entire React API before starting. The pattern above covers the
majority of what the v1 frontend requires.

---

## 12. Suggested implementation order

Work through these steps in order. Each step produces something visible and testable before
moving on.

1. **Create the Vite React app** — `npm create vite@latest apps/PhotoArchive.Web -- --template react-ts`
2. **Render a static layout** — add a header, navigation placeholder, and footer with hardcoded text
3. **Create a hardcoded YearCard** — build the component with fixed data to get the shape right
4. **Fetch `/years`** — replace the hardcoded data with a real API call in a `useEffect`
5. **Render year cards** — map over the API response and render a `YearCard` for each year
6. **Add routing** — install React Router and wire up the routes from section 8
7. **Add year / month / day pages** — implement `YearPage`, `MonthPage`, and `DayPage` following
   the same fetch-and-render pattern
8. **Add photo grid** — build `PhotoCard` and `PhotoGrid`; use them in `DayPage`
9. **Add photo detail** — implement `PhotoDetailPage`
10. **Add posts, galleries, and on-this-day** — implement the remaining page components

Each step is independent enough that you can stop after any of them and have a working application.

---

## 13. Deferred work

The following are explicitly out of scope for this phase and should not be introduced until a
later milestone:

| Deferred item | Reason |
|---|---|
| CRUD operations | Read-only archive in v1 |
| Photo uploads | Out of scope for frontend v1 |
| Authentication | Not required for a read-only public archive |
| Editing metadata | Deferred until admin workflows are planned |
| First-class Gallery entity | Gallery browsing is supported; entity management is not |
| Cover photo selection | Deferred to a later admin phase |
| Advanced search | Not in the v1 API scope |
| Server-side rendering / Next.js | Deferred until product direction requires it |
| Mobile app | A separate workstream if needed |
