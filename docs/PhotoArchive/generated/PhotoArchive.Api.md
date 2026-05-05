# API Reference
- [ArchiveController](#archivecontroller)
- [PhotosController](#photoscontroller)
- [Program](#program)

<a id="archivecontroller"></a>
# ArchiveController

Provides endpoints for navigating the photo archive by year, month, and day.

<a id="photoarchive.api.controllers.archivecontroller.#ctor(photoarchive.data.services.photoservice)"></a>
## Method: #ctor(PhotoService)
Initializes a new instance of [ArchiveController](#archivecontroller).

**Parameters**
- `photoService` — The photo data service.

<a id="photoarchive.api.controllers.archivecontroller.getday(int,int,int)"></a>
## Method: GetDay(int, int, int)
Returns the detail for a specific year/month/day, including photo count and navigation links.

**Parameters**
- `year` — The four-digit year.
- `month` — The month number (1–12).
- `day` — The day of the month (1–31).

**Returns**

A [DayDetailResponse](#daydetailresponse), or 404 if no photos exist on that date.

<a id="photoarchive.api.controllers.archivecontroller.getdays(int,int)"></a>
## Method: GetDays(int, int)
Returns the days within a given year/month that contain photos, along with a photo count and navigation links.

**Parameters**
- `year` — The four-digit year.
- `month` — The month number (1–12).

**Returns**

An object with `year`, `month`, `photoCount`, `links`, and a `days` array of [DaySummaryDto](#daysummarydto) items.

<a id="photoarchive.api.controllers.archivecontroller.getmonth(int,int)"></a>
## Method: GetMonth(int, int)
Returns the detail for a specific year/month, including its days and navigation links.

**Parameters**
- `year` — The four-digit year.
- `month` — The month number (1–12).

**Returns**

A [MonthDetailResponse](#monthdetailresponse), or 404 if no photos exist for that month.

<a id="photoarchive.api.controllers.archivecontroller.getmonths(int)"></a>
## Method: GetMonths(int)
Returns the months within a year that contain photos.

**Parameters**
- `year` — The four-digit year.

**Returns**

An object with `year` and a `months` array of [MonthSummaryDto](#monthsummarydto) items.

<a id="photoarchive.api.controllers.archivecontroller.getphotosforday(int,int,int,int,int)"></a>
## Method: GetPhotosForDay(int, int, int, int, int)
Returns a paged list of photos taken on the specified year/month/day.

**Parameters**
- `year` — The four-digit year.
- `month` — The month number (1–12).
- `day` — The day of the month (1–31).
- `page` — The 1-based page number. Defaults to 1.
- `pageSize` — The number of results per page. Defaults to 50.

**Returns**

A [PagedResponse<T1>](#pagedresponset1) of [PhotoDto](#photodto) items.

<a id="photoarchive.api.controllers.archivecontroller.getphotosformonth(int,int,int,int)"></a>
## Method: GetPhotosForMonth(int, int, int, int)
Returns a paged list of photos taken in the specified year and month.

**Parameters**
- `year` — The four-digit year.
- `month` — The month number (1–12).
- `page` — The 1-based page number. Defaults to 1.
- `pageSize` — The number of results per page. Defaults to 50.

**Returns**

A [PagedResponse<T1>](#pagedresponset1) of [PhotoDto](#photodto) items.

<a id="photoarchive.api.controllers.archivecontroller.getphotosforyear(int,int,int)"></a>
## Method: GetPhotosForYear(int, int, int)
Returns a paged list of photos taken in the specified year.

**Parameters**
- `year` — The four-digit year.
- `page` — The 1-based page number. Defaults to 1.
- `pageSize` — The number of results per page. Defaults to 50.

**Returns**

A [PagedResponse<T1>](#pagedresponset1) of [PhotoDto](#photodto) items.

<a id="photoarchive.api.controllers.archivecontroller.getyear(int)"></a>
## Method: GetYear(int)
Returns the detail for a specific year, including its months and navigation links.

**Parameters**
- `year` — The four-digit year.

**Returns**

A [YearDetailResponse](#yeardetailresponse), or 404 if no photos exist for that year.

<a id="photoarchive.api.controllers.archivecontroller.getyears"></a>
## Method: GetYears
Returns a summary of all years that contain photos in the archive.

**Returns**

An object with a `years` array of [YearSummaryDto](#yearsummarydto) items.


---

<a id="photoscontroller"></a>
# PhotosController

Provides endpoints for querying individual photos, galleries, posts, and the "On This Day" feature.

<a id="photoarchive.api.controllers.photoscontroller.#ctor(photoarchive.data.services.photoservice)"></a>
## Method: #ctor(PhotoService)
Initializes a new instance of [PhotosController](#photoscontroller).

**Parameters**
- `photoService` — The photo data service.

<a id="photoarchive.api.controllers.photoscontroller.getbygallery(string)"></a>
## Method: GetByGallery(string)
Returns all photos that belong to the specified gallery.

**Parameters**
- `gallery` — The gallery name.

**Returns**

A list of photos in the gallery.

<a id="photoarchive.api.controllers.photoscontroller.getbypost(string)"></a>
## Method: GetByPost(string)
Returns all photos associated with the specified blog post.

**Parameters**
- `postId` — The blog post identifier.

**Returns**

A list of photos for the post.

<a id="photoarchive.api.controllers.photoscontroller.getbyslug(string)"></a>
## Method: GetBySlug(string)
Returns the detail for a single photo identified by its slug.

**Parameters**
- `slug` — The unique slug of the photo.

**Returns**

A [PhotoDetailResponse](#photodetailresponse), or 404 if not found.

<a id="photoarchive.api.controllers.photoscontroller.getonthisday(system.nullable[int],system.nullable[int])"></a>
## Method: GetOnThisDay(Nullable<int>, Nullable<int>)
Returns photos grouped by year for a given month and day, defaulting to today's date if not specified.

**Parameters**
- `month` — Optional month override (1–12). Defaults to the current UTC month.
- `day` — Optional day override (1–31). Defaults to the current UTC day.

**Returns**

An [OnThisDayResponse](#onthisdayresponse) grouped by year.

<a id="photoarchive.api.controllers.photoscontroller.getphotos(system.nullable[int],system.nullable[int],system.nullable[int],int,int)"></a>
## Method: GetPhotos(Nullable<int>, Nullable<int>, Nullable<int>, int, int)
Returns a paged list of photos, optionally filtered by year, month, and/or day.

**Parameters**
- `year` — Optional year filter.
- `month` — Optional month filter (1–12).
- `day` — Optional day filter (1–31).
- `page` — The 1-based page number. Defaults to 1.
- `pageSize` — The number of results per page. Defaults to 50.

**Returns**

A paged list of photos matching the supplied filters.

<a id="photoarchive.api.controllers.photoscontroller.getpost(string)"></a>
## Method: GetPost(string)
Returns a summary for the specified blog post, including photo count and navigation links.

**Parameters**
- `postId` — The blog post identifier.

**Returns**

A [PostSummaryResponse](#postsummaryresponse), or 404 if no photos are associated with the post.


---

<a id="program"></a>
# Program

Auto-generated public partial Program class for top-level statement apps.

