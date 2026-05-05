# API Reference
- [Photo](#photo)
- [PhotoDtoMapper](#photodtomapper)
- [ApiLink](#apilink)
- [DayDetailResponse](#daydetailresponse)
- [DaySummaryDto](#daysummarydto)
- [MonthDetailResponse](#monthdetailresponse)
- [MonthSummaryDto](#monthsummarydto)
- [OnThisDayResponse](#onthisdayresponse)
- [OnThisDayYearGroup](#onthisdayyeargroup)
- [PagedResponse<T1>](#pagedresponset1)
- [PhotoDetailResponse](#photodetailresponse)
- [PhotoDto](#photodto)
- [PostSummaryResponse](#postsummaryresponse)
- [YearDetailResponse](#yeardetailresponse)
- [YearSummaryDto](#yearsummarydto)

<a id="photo"></a>
# Photo

Represents a single photo record stored in the archive.

<a id="photoarchive.core.entities.photo.createdat"></a>
## Property: CreatedAt
Gets or sets the date and time this record was created in the archive.

<a id="photoarchive.core.entities.photo.day"></a>
## Property: Day
Gets or sets the day of the month the photo was taken, derived from [TakenAt](#photoarchive.core.entities.photo.takenat).

<a id="photoarchive.core.entities.photo.gallery"></a>
## Property: Gallery
Gets or sets the name of the gallery this photo belongs to, if any.

<a id="photoarchive.core.entities.photo.id"></a>
## Property: Id
Gets or sets the unique identifier for the photo.

<a id="photoarchive.core.entities.photo.month"></a>
## Property: Month
Gets or sets the month the photo was taken, derived from [TakenAt](#photoarchive.core.entities.photo.takenat).

<a id="photoarchive.core.entities.photo.originalurl"></a>
## Property: OriginalUrl
Gets or sets the URL to the full-resolution image in Azure Blob Storage.

<a id="photoarchive.core.entities.photo.postid"></a>
## Property: PostId
Gets or sets the identifier of the blog post associated with this photo, if any.

<a id="photoarchive.core.entities.photo.posturl"></a>
## Property: PostUrl
Gets or sets the URL of the blog post associated with this photo, if any.

<a id="photoarchive.core.entities.photo.slug"></a>
## Property: Slug
Gets or sets the URL-safe unique slug used to identify the photo.

<a id="photoarchive.core.entities.photo.sortindex"></a>
## Property: SortIndex
Gets or sets the sort order of this photo within its gallery or post.

<a id="photoarchive.core.entities.photo.source"></a>
## Property: Source
Gets or sets the import source identifier (e.g., "legacy", "facebook").

<a id="photoarchive.core.entities.photo.sourcefilename"></a>
## Property: SourceFilename
Gets or sets the original source filename of the photo, if known.

<a id="photoarchive.core.entities.photo.sourcemetadatajson"></a>
## Property: SourceMetadataJson
Gets or sets the raw YAML front-matter serialized as JSON for auditing purposes.

<a id="photoarchive.core.entities.photo.takenat"></a>
## Property: TakenAt
Gets or sets the date and time the photo was taken.

<a id="photoarchive.core.entities.photo.thumburl"></a>
## Property: ThumbUrl
Gets or sets the URL to the thumbnail image, if available.

<a id="photoarchive.core.entities.photo.title"></a>
## Property: Title
Gets or sets the optional display title of the photo.

<a id="photoarchive.core.entities.photo.year"></a>
## Property: Year
Gets or sets the year the photo was taken, derived from [TakenAt](#photoarchive.core.entities.photo.takenat).


---

<a id="photodtomapper"></a>
# PhotoDtoMapper

Provides mapping from [Photo](#photo) entities to [PhotoDto](#photodto) data transfer objects.

<a id="photoarchive.core.mapping.photodtomapper.todto(photoarchive.core.entities.photo)"></a>
## Method: ToDto(Photo)
Maps a [Photo](#photo) entity to a [PhotoDto](#photodto).

**Parameters**
- `photo` — The source photo entity.

**Returns**

A new [PhotoDto](#photodto) populated from the entity.


---

<a id="apilink"></a>
# ApiLink

Represents a hypermedia link included in API responses.

<a id="photoarchive.core.models.apilink.href"></a>
## Property: Href
Gets or sets the URL of the link.


---

<a id="daydetailresponse"></a>
# DayDetailResponse

The detail response for a specific calendar day, including photo count and navigation links.

<a id="photoarchive.core.models.daydetailresponse.day"></a>
## Property: Day
Gets or sets the day component of the date (1–31).

<a id="photoarchive.core.models.daydetailresponse.links"></a>
## Property: Links
Gets or sets the hypermedia navigation links for this day. Includes: `self`, `photos`, `query`, `onThisDay`.

<a id="photoarchive.core.models.daydetailresponse.month"></a>
## Property: Month
Gets or sets the month component of the date (1–12).

<a id="photoarchive.core.models.daydetailresponse.photocount"></a>
## Property: PhotoCount
Gets or sets the number of photos taken on this day.

<a id="photoarchive.core.models.daydetailresponse.year"></a>
## Property: Year
Gets or sets the year component of the date.


---

<a id="daysummarydto"></a>
# DaySummaryDto

A summary data transfer object for a single calendar day within a month listing.

<a id="photoarchive.core.models.daysummarydto.day"></a>
## Property: Day
Gets or sets the day component of the date (1–31).

<a id="photoarchive.core.models.daysummarydto.links"></a>
## Property: Links
Gets or sets the hypermedia navigation links for this day. Includes: `self`, `photos`, `query`, `onThisDay`.

<a id="photoarchive.core.models.daysummarydto.month"></a>
## Property: Month
Gets or sets the month component of the date (1–12).

<a id="photoarchive.core.models.daysummarydto.photocount"></a>
## Property: PhotoCount
Gets or sets the number of photos taken on this day.

<a id="photoarchive.core.models.daysummarydto.year"></a>
## Property: Year
Gets or sets the year component of the date.


---

<a id="monthdetailresponse"></a>
# MonthDetailResponse

The detail response for a specific calendar month, including its days and navigation links.

<a id="photoarchive.core.models.monthdetailresponse.days"></a>
## Property: Days
Gets or sets the summary list of days within this month that contain photos.

<a id="photoarchive.core.models.monthdetailresponse.links"></a>
## Property: Links
Gets or sets the hypermedia navigation links for this month. Includes: `self`, `days`, `photos`, `query`.

<a id="photoarchive.core.models.monthdetailresponse.month"></a>
## Property: Month
Gets or sets the month number (1–12).

<a id="photoarchive.core.models.monthdetailresponse.photocount"></a>
## Property: PhotoCount
Gets or sets the total number of photos taken during this month.

<a id="photoarchive.core.models.monthdetailresponse.year"></a>
## Property: Year
Gets or sets the year of the month.


---

<a id="monthsummarydto"></a>
# MonthSummaryDto

A summary data transfer object for a single calendar month within a year listing.

<a id="photoarchive.core.models.monthsummarydto.links"></a>
## Property: Links
Gets or sets the hypermedia navigation links for this month. Includes: `self`, `days`, `photos`, `query`.

<a id="photoarchive.core.models.monthsummarydto.month"></a>
## Property: Month
Gets or sets the month number (1–12).

<a id="photoarchive.core.models.monthsummarydto.photocount"></a>
## Property: PhotoCount
Gets or sets the total number of photos taken during this month.

<a id="photoarchive.core.models.monthsummarydto.year"></a>
## Property: Year
Gets or sets the year of the month.


---

<a id="onthisdayresponse"></a>
# OnThisDayResponse

The response returned by the "On This Day" endpoint, grouping photos by year for a given month/day.

<a id="photoarchive.core.models.onthisdayresponse.day"></a>
## Property: Day
Gets or sets the day of the month of the query (1–31).

<a id="photoarchive.core.models.onthisdayresponse.month"></a>
## Property: Month
Gets or sets the month of the query (1–12).

<a id="photoarchive.core.models.onthisdayresponse.years"></a>
## Property: Years
Gets or sets the list of year groups, ordered from most recent to oldest.


---

<a id="onthisdayyeargroup"></a>
# OnThisDayYearGroup

A group of photos taken on the same month/day in a specific year.

<a id="photoarchive.core.models.onthisdayyeargroup.count"></a>
## Property: Count
Gets the number of photos in this year group.

<a id="photoarchive.core.models.onthisdayyeargroup.photos"></a>
## Property: Photos
Gets or sets the photos taken on this day in this year.

<a id="photoarchive.core.models.onthisdayyeargroup.year"></a>
## Property: Year
Gets or sets the year this group represents.


---

<a id="pagedresponset1"></a>
# PagedResponse<T1>

A generic paginated response wrapper returned by collection endpoints.

<a id="photoarchive.core.models.pagedresponse`1.items"></a>
## Property: Items
Gets or sets the items on the current page.

<a id="photoarchive.core.models.pagedresponse`1.links"></a>
## Property: Links
Gets or sets the hypermedia navigation links for the paged result. Common keys: `self`, `first`, `last`, `previous`, `next`.

<a id="photoarchive.core.models.pagedresponse`1.page"></a>
## Property: Page
Gets or sets the current page number (1-based).

<a id="photoarchive.core.models.pagedresponse`1.pagesize"></a>
## Property: PageSize
Gets or sets the maximum number of items per page.

<a id="photoarchive.core.models.pagedresponse`1.totalcount"></a>
## Property: TotalCount
Gets or sets the total number of matching items across all pages.

<a id="photoarchive.core.models.pagedresponse`1.totalpages"></a>
## Property: TotalPages
Gets or sets the total number of pages available.


---

<a id="photodetailresponse"></a>
# PhotoDetailResponse

The full detail response returned for a single photo, including hypermedia links.

<a id="photoarchive.core.models.photodetailresponse.links"></a>
## Property: Links
Gets or sets the hypermedia navigation links for this photo. May include: `self`, `sameDate`, `onThisDay`, `gallery`, `galleryPhotos`, `post`, `postPhotos`.

<a id="photoarchive.core.models.photodetailresponse.photo"></a>
## Property: Photo
Gets or sets the photo data transfer object.


---

<a id="photodto"></a>
# PhotoDto

A lightweight data transfer object representing a photo in list and summary contexts.

<a id="photoarchive.core.models.photodto.day"></a>
## Property: Day
Gets or sets the day of the month the photo was taken.

<a id="photoarchive.core.models.photodto.gallery"></a>
## Property: Gallery
Gets or sets the gallery name this photo belongs to, if any.

<a id="photoarchive.core.models.photodto.month"></a>
## Property: Month
Gets or sets the month the photo was taken.

<a id="photoarchive.core.models.photodto.originalurl"></a>
## Property: OriginalUrl
Gets or sets the URL to the full-resolution image.

<a id="photoarchive.core.models.photodto.postid"></a>
## Property: PostId
Gets or sets the identifier of the associated blog post, if any.

<a id="photoarchive.core.models.photodto.posturl"></a>
## Property: PostUrl
Gets or sets the URL of the associated blog post, if any.

<a id="photoarchive.core.models.photodto.slug"></a>
## Property: Slug
Gets or sets the URL-safe unique slug that identifies the photo.

<a id="photoarchive.core.models.photodto.sortindex"></a>
## Property: SortIndex
Gets or sets the sort position within a gallery or post.

<a id="photoarchive.core.models.photodto.source"></a>
## Property: Source
Gets or sets the import source identifier (e.g., "legacy", "facebook").

<a id="photoarchive.core.models.photodto.takenat"></a>
## Property: TakenAt
Gets or sets the date and time the photo was taken.

<a id="photoarchive.core.models.photodto.thumburl"></a>
## Property: ThumbUrl
Gets or sets the URL to the thumbnail image, if available.

<a id="photoarchive.core.models.photodto.title"></a>
## Property: Title
Gets or sets the optional display title of the photo.

<a id="photoarchive.core.models.photodto.year"></a>
## Property: Year
Gets or sets the year the photo was taken.


---

<a id="postsummaryresponse"></a>
# PostSummaryResponse

The response returned for a blog post, summarising the photos associated with it.

<a id="photoarchive.core.models.postsummaryresponse.links"></a>
## Property: Links
Gets or sets the hypermedia navigation links for this post. Includes: `self`, `photos`.

<a id="photoarchive.core.models.postsummaryresponse.photocount"></a>
## Property: PhotoCount
Gets or sets the total number of photos associated with this post.

<a id="photoarchive.core.models.postsummaryresponse.postid"></a>
## Property: PostId
Gets or sets the identifier of the blog post.

<a id="photoarchive.core.models.postsummaryresponse.posturl"></a>
## Property: PostUrl
Gets or sets the URL of the blog post, if available.


---

<a id="yeardetailresponse"></a>
# YearDetailResponse

The detail response for a specific calendar year, including its months and navigation links.

<a id="photoarchive.core.models.yeardetailresponse.links"></a>
## Property: Links
Gets or sets the hypermedia navigation links for this year. Includes: `self`, `months`, `photos`, `query`.

<a id="photoarchive.core.models.yeardetailresponse.months"></a>
## Property: Months
Gets or sets the summary list of months within this year that contain photos.

<a id="photoarchive.core.models.yeardetailresponse.photocount"></a>
## Property: PhotoCount
Gets or sets the total number of photos taken during this year.

<a id="photoarchive.core.models.yeardetailresponse.year"></a>
## Property: Year
Gets or sets the year.


---

<a id="yearsummarydto"></a>
# YearSummaryDto

A summary data transfer object for a single calendar year in the archive index.

<a id="photoarchive.core.models.yearsummarydto.links"></a>
## Property: Links
Gets or sets the hypermedia navigation links for this year. Includes: `months`, `photos`, `query`.

<a id="photoarchive.core.models.yearsummarydto.photocount"></a>
## Property: PhotoCount
Gets or sets the total number of photos taken during this year.

<a id="photoarchive.core.models.yearsummarydto.year"></a>
## Property: Year
Gets or sets the year.

