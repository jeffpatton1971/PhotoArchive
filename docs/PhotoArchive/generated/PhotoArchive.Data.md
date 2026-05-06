# API Reference
- [AddPhotoImportFields](#addphotoimportfields)
- [AddPhotoQueryIndexes](#addphotoqueryindexes)
- [AddPhotoSourceMetadataJson](#addphotosourcemetadatajson)
- [InitialCreate](#initialcreate)
- [PhotoDbContext](#photodbcontext)
- [PhotoService](#photoservice)

<a id="addphotoimportfields"></a>
# AddPhotoImportFields

<a id="photoarchive.data.migrations.addphotoimportfields.buildtargetmodel(microsoft.entityframeworkcore.modelbuilder)"></a>
## Method: BuildTargetModel(ModelBuilder)

<a id="photoarchive.data.migrations.addphotoimportfields.down(microsoft.entityframeworkcore.migrations.migrationbuilder)"></a>
## Method: Down(MigrationBuilder)

<a id="photoarchive.data.migrations.addphotoimportfields.up(microsoft.entityframeworkcore.migrations.migrationbuilder)"></a>
## Method: Up(MigrationBuilder)


---

<a id="addphotoqueryindexes"></a>
# AddPhotoQueryIndexes

<a id="photoarchive.data.migrations.addphotoqueryindexes.buildtargetmodel(microsoft.entityframeworkcore.modelbuilder)"></a>
## Method: BuildTargetModel(ModelBuilder)

<a id="photoarchive.data.migrations.addphotoqueryindexes.down(microsoft.entityframeworkcore.migrations.migrationbuilder)"></a>
## Method: Down(MigrationBuilder)

<a id="photoarchive.data.migrations.addphotoqueryindexes.up(microsoft.entityframeworkcore.migrations.migrationbuilder)"></a>
## Method: Up(MigrationBuilder)


---

<a id="addphotosourcemetadatajson"></a>
# AddPhotoSourceMetadataJson

<a id="photoarchive.data.migrations.addphotosourcemetadatajson.buildtargetmodel(microsoft.entityframeworkcore.modelbuilder)"></a>
## Method: BuildTargetModel(ModelBuilder)

<a id="photoarchive.data.migrations.addphotosourcemetadatajson.down(microsoft.entityframeworkcore.migrations.migrationbuilder)"></a>
## Method: Down(MigrationBuilder)

<a id="photoarchive.data.migrations.addphotosourcemetadatajson.up(microsoft.entityframeworkcore.migrations.migrationbuilder)"></a>
## Method: Up(MigrationBuilder)


---

<a id="initialcreate"></a>
# InitialCreate

<a id="photoarchive.data.migrations.initialcreate.buildtargetmodel(microsoft.entityframeworkcore.modelbuilder)"></a>
## Method: BuildTargetModel(ModelBuilder)

<a id="photoarchive.data.migrations.initialcreate.down(microsoft.entityframeworkcore.migrations.migrationbuilder)"></a>
## Method: Down(MigrationBuilder)

<a id="photoarchive.data.migrations.initialcreate.up(microsoft.entityframeworkcore.migrations.migrationbuilder)"></a>
## Method: Up(MigrationBuilder)


---

<a id="photodbcontext"></a>
# PhotoDbContext

The Entity Framework Core database context for the PhotoArchive PostgreSQL database.

<a id="photoarchive.data.photodbcontext.#ctor(microsoft.entityframeworkcore.dbcontextoptions[photoarchive.data.photodbcontext])"></a>
## Method: #ctor(DbContextOptions<PhotoDbContext>)
Initializes a new instance of [PhotoDbContext](#photodbcontext) with the given options.

**Parameters**
- `options` — The options used to configure the context.

<a id="photoarchive.data.photodbcontext.onmodelcreating(microsoft.entityframeworkcore.modelbuilder)"></a>
## Method: OnModelCreating(ModelBuilder)

<a id="photoarchive.data.photodbcontext.photos"></a>
## Property: Photos
Gets the [DbSet<T1>](#dbsett1) for [Photo](#photo) records.


---

<a id="photoservice"></a>
# PhotoService

Provides data-access operations for photos, including retrieval by date, gallery, post, and slug.

<a id="photoarchive.data.services.photoservice.#ctor(photoarchive.data.photodbcontext)"></a>
## Method: #ctor(PhotoDbContext)
Initializes a new instance of [PhotoService](#photoservice) with the given database context.

**Parameters**
- `db` — The [PhotoDbContext](#photodbcontext) to use for data access.

<a id="photoarchive.data.services.photoservice.buildphotoquery(photoarchive.core.models.photoqueryoptions)"></a>
## Method: BuildPhotoQuery(PhotoQueryOptions)
Builds an [IQueryable<T1>](#iqueryablet1) of [Photo](#photo) records filtered by the supplied `options`. All photo collection queries are routed through this method to ensure consistent filtering behaviour across every endpoint.

**Parameters**
- `options` — The filter and pagination options.

**Returns**

A filtered, unexecuted [IQueryable<T1>](#iqueryablet1).

<a id="photoarchive.data.services.photoservice.getbygalleryasync(string,int,int)"></a>
## Method: GetByGalleryAsync(string, int, int)
Returns a paged list of photos belonging to the specified gallery. Delegates to [GetPhotosByQueryAsync(PhotoQueryOptions, string)](#photoarchive.data.services.photoservice.getphotosbyqueryasync(photoarchive.core.models.photoqueryoptions,string)) so that all collection endpoints share the same filtering path.

**Parameters**
- `gallery` — The gallery name.
- `page` — The 1-based page number to retrieve. Defaults to 1.
- `pageSize` — The maximum number of results per page. Defaults to 50.

**Returns**

A [PagedResponse<T1>](#pagedresponset1) of [PhotoDto](#photodto) items for the gallery.

<a id="photoarchive.data.services.photoservice.getbypostasync(string,int,int)"></a>
## Method: GetByPostAsync(string, int, int)
Returns a paged list of photos associated with the specified blog post. Delegates to [GetPhotosByQueryAsync(PhotoQueryOptions, string)](#photoarchive.data.services.photoservice.getphotosbyqueryasync(photoarchive.core.models.photoqueryoptions,string)) so that all collection endpoints share the same filtering path.

**Parameters**
- `postId` — The blog post identifier.
- `page` — The 1-based page number to retrieve. Defaults to 1.
- `pageSize` — The maximum number of results per page. Defaults to 50.

**Returns**

A [PagedResponse<T1>](#pagedresponset1) of [PhotoDto](#photodto) items for the post.

<a id="photoarchive.data.services.photoservice.getbyslugasync(string)"></a>
## Method: GetBySlugAsync(string)
Returns the [PhotoDto](#photodto) for the photo with the given slug, or if not found.

**Parameters**
- `slug` — The unique slug of the photo.

**Returns**

A [PhotoDto](#photodto), or if no matching photo exists.

<a id="photoarchive.data.services.photoservice.getdayasync(int,int,int)"></a>
## Method: GetDayAsync(int, int, int)
Returns the detail for a specific year/month/day, or if no photos exist on that date.

**Parameters**
- `year` — The four-digit year.
- `month` — The month number (1–12).
- `day` — The day of the month (1–31).

**Returns**

A [DayDetailResponse](#daydetailresponse), or.

<a id="photoarchive.data.services.photoservice.getdaysasync(int,int)"></a>
## Method: GetDaysAsync(int, int)
Returns the list of days within a given year/month that have at least one photo.

**Parameters**
- `year` — The four-digit year.
- `month` — The month number (1–12).

**Returns**

A list of [DaySummaryDto](#daysummarydto) items ordered by day number.

<a id="photoarchive.data.services.photoservice.getmonthasync(int,int)"></a>
## Method: GetMonthAsync(int, int)
Returns the detail for a specific year/month, including its days and navigation links, or if no photos exist.

**Parameters**
- `year` — The four-digit year.
- `month` — The month number (1–12).

**Returns**

A [MonthDetailResponse](#monthdetailresponse), or.

<a id="photoarchive.data.services.photoservice.getmonthsasync(int)"></a>
## Method: GetMonthsAsync(int)
Returns the list of months within the given year that have at least one photo.

**Parameters**
- `year` — The four-digit year.

**Returns**

A list of [MonthSummaryDto](#monthsummarydto) items ordered by month number.

<a id="photoarchive.data.services.photoservice.getonthisdayasync(int,int)"></a>
## Method: GetOnThisDayAsync(int, int)
Returns up to 500 photos taken on the given month and day across all years, ordered by most recent year first.

**Parameters**
- `month` — The month (1–12).
- `day` — The day of the month (1–31).

**Returns**

A flat list of [PhotoDto](#photodto) items.

<a id="photoarchive.data.services.photoservice.getonthisdaygroupedasync(int,int)"></a>
## Method: GetOnThisDayGroupedAsync(int, int)
Returns an [OnThisDayResponse](#onthisdayresponse) that groups photos taken on the given month/day by year.

**Parameters**
- `month` — The month (1–12).
- `day` — The day of the month (1–31).

**Returns**

An [OnThisDayResponse](#onthisdayresponse) with photos grouped by year, most recent first.

<a id="photoarchive.data.services.photoservice.getphotodetailasync(string)"></a>
## Method: GetPhotoDetailAsync(string)
Returns the full detail for a single photo identified by its slug, or if not found.

**Parameters**
- `slug` — The unique slug of the photo.

**Returns**

A [PhotoDetailResponse](#photodetailresponse) with the photo and its hypermedia links, or.

<a id="photoarchive.data.services.photoservice.getphotosasync(system.nullable[int],system.nullable[int],system.nullable[int],int,int,string,string,string)"></a>
## Method: GetPhotosAsync(Nullable<int>, Nullable<int>, Nullable<int>, int, int, string, string, string)
Returns a paged list of photos filtered by optional year, month, day, source, gallery, and post. Delegates to [GetPhotosByQueryAsync(PhotoQueryOptions, string)](#photoarchive.data.services.photoservice.getphotosbyqueryasync(photoarchive.core.models.photoqueryoptions,string)) so that all collection endpoints share the same filtering path.

**Parameters**
- `year` — Optional year filter.
- `month` — Optional month filter (1–12).
- `day` — Optional day filter (1–31).
- `source` — Optional import source filter.
- `gallery` — Optional gallery name filter.
- `postId` — Optional blog post identifier filter.
- `page` — The 1-based page number to retrieve. Defaults to 1.
- `pageSize` — The maximum number of results per page. Defaults to 50.

**Returns**

A [PagedResponse<T1>](#pagedresponset1) containing the matching [PhotoDto](#photodto) items.

<a id="photoarchive.data.services.photoservice.getphotosbydateasync(int,int,int)"></a>
## Method: GetPhotosByDateAsync(int, int, int)
Returns all photos taken on a specific date, ordered by sort index then taken time.

**Parameters**
- `year` — The four-digit year.
- `month` — The month number (1–12).
- `day` — The day of the month (1–31).

**Returns**

A list of [PhotoDto](#photodto) items for the specified date.

<a id="photoarchive.data.services.photoservice.getphotosbydayasync(int,int,int,int,int)"></a>
## Method: GetPhotosByDayAsync(int, int, int, int, int)
Returns a paged list of photos taken on the specified year, month, and day. Delegates to [GetPhotosByQueryAsync(PhotoQueryOptions, string)](#photoarchive.data.services.photoservice.getphotosbyqueryasync(photoarchive.core.models.photoqueryoptions,string)) so that all collection endpoints share the same filtering path.

**Parameters**
- `year` — The four-digit year.
- `month` — The month number (1–12).
- `day` — The day of the month (1–31).
- `page` — The 1-based page number to retrieve.
- `pageSize` — The maximum number of results per page.

**Returns**

A [PagedResponse<T1>](#pagedresponset1) containing the matching [PhotoDto](#photodto) items.

<a id="photoarchive.data.services.photoservice.getphotosbymonthasync(int,int,int,int)"></a>
## Method: GetPhotosByMonthAsync(int, int, int, int)
Returns a paged list of photos taken in the specified year and month. Delegates to [GetPhotosByQueryAsync(PhotoQueryOptions, string)](#photoarchive.data.services.photoservice.getphotosbyqueryasync(photoarchive.core.models.photoqueryoptions,string)) so that all collection endpoints share the same filtering path.

**Parameters**
- `year` — The four-digit year.
- `month` — The month number (1–12).
- `page` — The 1-based page number to retrieve.
- `pageSize` — The maximum number of results per page.

**Returns**

A [PagedResponse<T1>](#pagedresponset1) containing the matching [PhotoDto](#photodto) items.

<a id="photoarchive.data.services.photoservice.getphotosbyqueryasync(photoarchive.core.models.photoqueryoptions,string)"></a>
## Method: GetPhotosByQueryAsync(PhotoQueryOptions, string)
Returns a paged list of photos matching the supplied `options`. All photo collection endpoints route through this method. When filtering by gallery or post, photos are ordered by `SortIndex` then `TakenAt` to preserve authored sequence. All other queries order by most-recent `TakenAt` first.

**Parameters**
- `options` — The filter and pagination options.
- `path` — The canonical URL path used for link generation (without pagination query params).

**Returns**

A [PagedResponse<T1>](#pagedresponset1) of [PhotoDto](#photodto) items.

<a id="photoarchive.data.services.photoservice.getphotosbyyearasync(int,int,int)"></a>
## Method: GetPhotosByYearAsync(int, int, int)
Returns a paged list of photos taken in the specified year. Delegates to [GetPhotosByQueryAsync(PhotoQueryOptions, string)](#photoarchive.data.services.photoservice.getphotosbyqueryasync(photoarchive.core.models.photoqueryoptions,string)) so that all collection endpoints share the same filtering path.

**Parameters**
- `year` — The four-digit year.
- `page` — The 1-based page number to retrieve.
- `pageSize` — The maximum number of results per page.

**Returns**

A [PagedResponse<T1>](#pagedresponset1) containing the matching [PhotoDto](#photodto) items.

<a id="photoarchive.data.services.photoservice.getpostsummaryasync(string)"></a>
## Method: GetPostSummaryAsync(string)
Returns a summary for a blog post identified by `postId`, or if no photos are associated.

**Parameters**
- `postId` — The identifier of the blog post.

**Returns**

A [PostSummaryResponse](#postsummaryresponse) with post metadata and navigation links, or.

<a id="photoarchive.data.services.photoservice.getyearasync(int)"></a>
## Method: GetYearAsync(int)
Returns the detail for the given year, including its months and navigation links, or if no photos exist for that year.

**Parameters**
- `year` — The four-digit year.

**Returns**

A [YearDetailResponse](#yeardetailresponse), or.

<a id="photoarchive.data.services.photoservice.getyearsasync"></a>
## Method: GetYearsAsync
Returns the list of years that have at least one photo in the archive.

**Returns**

A list of [YearSummaryDto](#yearsummarydto) items ordered from most recent year to oldest.

