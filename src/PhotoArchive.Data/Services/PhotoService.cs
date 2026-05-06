using Microsoft.EntityFrameworkCore;
using PhotoArchive.Core.Entities;
using PhotoArchive.Core.Models;
using PhotoArchive.Core.Mapping;

namespace PhotoArchive.Data.Services;

/// <summary>
/// Provides data-access operations for photos, including retrieval by date, gallery, post, and slug.
/// </summary>
public class PhotoService
{
    private readonly PhotoDbContext _db;

    /// <summary>
    /// Initializes a new instance of <see cref="PhotoService"/> with the given database context.
    /// </summary>
    /// <param name="db">The <see cref="PhotoDbContext"/> to use for data access.</param>
    public PhotoService(PhotoDbContext db)
    {
        _db = db;
    }

    private static string BuildPhotoPath(int? year, int? month, int? day, string? source = null, string? gallery = null, string? postId = null)
    {
        var parameters = new List<string>();

        if (year.HasValue)
            parameters.Add($"year={year.Value}");

        if (month.HasValue)
            parameters.Add($"month={month.Value}");

        if (day.HasValue)
            parameters.Add($"day={day.Value}");

        if (!string.IsNullOrWhiteSpace(source))
            parameters.Add($"source={Uri.EscapeDataString(source)}");

        if (!string.IsNullOrWhiteSpace(gallery))
            parameters.Add($"gallery={Uri.EscapeDataString(gallery)}");

        if (!string.IsNullOrWhiteSpace(postId))
            parameters.Add($"postId={Uri.EscapeDataString(postId)}");

        if (parameters.Count == 0)
            return "/photos";

        return "/photos?" + string.Join("&", parameters);
    }

    /// <summary>
    /// Builds an <see cref="IQueryable{T}"/> of <see cref="Photo"/> records filtered by the supplied
    /// <paramref name="options"/>. All photo collection queries are routed through this method to ensure
    /// consistent filtering behaviour across every endpoint.
    /// </summary>
    /// <param name="options">The filter and pagination options.</param>
    /// <returns>A filtered, unexecuted <see cref="IQueryable{Photo}"/>.</returns>
    private IQueryable<Photo> BuildPhotoQuery(PhotoQueryOptions options)
    {
        var query = _db.Photos.AsQueryable();

        if (options.Year.HasValue)
            query = query.Where(p => p.Year == options.Year.Value);

        if (options.Month.HasValue)
            query = query.Where(p => p.Month == options.Month.Value);

        if (options.Day.HasValue)
            query = query.Where(p => p.Day == options.Day.Value);

        if (!string.IsNullOrWhiteSpace(options.Source))
            query = query.Where(p => p.Source == options.Source);

        if (!string.IsNullOrWhiteSpace(options.Gallery))
            query = query.Where(p => p.Gallery == options.Gallery);

        if (!string.IsNullOrWhiteSpace(options.PostId))
            query = query.Where(p => p.PostId == options.PostId);

        return query;
    }

    /// <summary>
    /// Returns a paged list of photos matching the supplied <paramref name="options"/>.
    /// All photo collection endpoints route through this method.
    /// When filtering by gallery or post, photos are ordered by <c>SortIndex</c> then <c>TakenAt</c>
    /// to preserve authored sequence. All other queries order by most-recent <c>TakenAt</c> first.
    /// </summary>
    /// <param name="options">The filter and pagination options.</param>
    /// <param name="path">The canonical URL path used for link generation (without pagination query params).</param>
    /// <returns>A <see cref="PagedResponse{T}"/> of <see cref="PhotoDto"/> items.</returns>
    public Task<PagedResponse<PhotoDto>> GetPhotosByQueryAsync(PhotoQueryOptions options, string path)
    {
        var query = BuildPhotoQuery(options);

        // Gallery and post collections preserve their authored sort order (SortIndex → TakenAt asc).
        // All other archive queries return most-recent photos first (TakenAt desc → SortIndex asc).
        var useAuthoredOrder = !string.IsNullOrWhiteSpace(options.Gallery)
                            || !string.IsNullOrWhiteSpace(options.PostId);

        return GetPagedPhotosAsync(query, path, options.Page, options.PageSize, useAuthoredOrder);
    }

    private async Task<PagedResponse<PhotoDto>> GetPagedPhotosAsync(
        IQueryable<Photo> query,
        string path,
        int page,
        int pageSize,
        bool useAuthoredOrder = false)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 250);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        // Gallery and post collections use authored order (SortIndex → TakenAt asc).
        // Archive and general queries use most-recent-first order (TakenAt desc → SortIndex asc).
        var orderedQuery = useAuthoredOrder
            ? query.OrderBy(p => p.SortIndex ?? int.MaxValue).ThenBy(p => p.TakenAt)
            : query.OrderByDescending(p => p.TakenAt).ThenBy(p => p.SortIndex ?? int.MaxValue);

        var photos = await orderedQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var links = PaginationLinkBuilder.Build(path, page, pageSize, totalPages);

        return new PagedResponse<PhotoDto>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            Items = photos.Select(PhotoDtoMapper.ToDto).ToList(),
            Links = links
        };
    }

    /// <summary>
    /// Returns a paged list of photos filtered by optional year, month, day, source, gallery, and post.
    /// Delegates to <see cref="GetPhotosByQueryAsync"/> so that all collection endpoints share the same filtering path.
    /// </summary>
    /// <param name="year">Optional year filter.</param>
    /// <param name="month">Optional month filter (1–12).</param>
    /// <param name="day">Optional day filter (1–31).</param>
    /// <param name="source">Optional import source filter.</param>
    /// <param name="gallery">Optional gallery name filter.</param>
    /// <param name="postId">Optional blog post identifier filter.</param>
    /// <param name="page">The 1-based page number to retrieve. Defaults to 1.</param>
    /// <param name="pageSize">The maximum number of results per page. Defaults to 50.</param>
    /// <returns>A <see cref="PagedResponse{T}"/> containing the matching <see cref="PhotoDto"/> items.</returns>
    public Task<PagedResponse<PhotoDto>> GetPhotosAsync(
        int? year,
        int? month,
        int? day,
        int page = 1,
        int pageSize = 50,
        string? source = null,
        string? gallery = null,
        string? postId = null)
    {
        var options = new PhotoQueryOptions
        {
            Year = year,
            Month = month,
            Day = day,
            Source = source,
            Gallery = gallery,
            PostId = postId,
            Page = page,
            PageSize = pageSize
        };

        return GetPhotosByQueryAsync(options, BuildPhotoPath(year, month, day, source, gallery, postId));
    }

    /// <summary>
    /// Returns the full detail for a single photo identified by its slug, or <see langword="null"/> if not found.
    /// </summary>
    /// <param name="slug">The unique slug of the photo.</param>
    /// <returns>A <see cref="PhotoDetailResponse"/> with the photo and its hypermedia links, or <see langword="null"/>.</returns>
    public async Task<PhotoDetailResponse?> GetPhotoDetailAsync(string slug)
    {
        var photo = await GetBySlugAsync(slug);

        if (photo is null)
            return null;

        var links = new Dictionary<string, ApiLink>
        {
            ["self"] = new() { Href = $"/photos/{photo.Slug}" }
        };

        if (photo.Year.HasValue && photo.Month.HasValue && photo.Day.HasValue)
        {
            links["sameDate"] = new()
            {
                Href = $"/photos?year={photo.Year}&month={photo.Month}&day={photo.Day}"
            };

            links["onThisDay"] = new()
            {
                Href = $"/on-this-day?month={photo.Month}&day={photo.Day}"
            };
        }

        if (!string.IsNullOrWhiteSpace(photo.Gallery))
        {
            links["gallery"] = new()
            {
                Href = $"/galleries/{Uri.EscapeDataString(photo.Gallery)}"
            };

            links["galleryPhotos"] = new()
            {
                Href = $"/galleries/{Uri.EscapeDataString(photo.Gallery)}/photos"
            };
        }

        if (!string.IsNullOrWhiteSpace(photo.PostId))
        {
            links["post"] = new()
            {
                Href = $"/posts/{Uri.EscapeDataString(photo.PostId)}"
            };

            links["postPhotos"] = new()
            {
                Href = $"/posts/{Uri.EscapeDataString(photo.PostId)}/photos"
            };
        }

        return new PhotoDetailResponse
        {
            Photo = photo,
            Links = links
        };
    }

    /// <summary>
    /// Returns a summary for a blog post identified by <paramref name="postId"/>, or <see langword="null"/> if no photos are associated.
    /// </summary>
    /// <param name="postId">The identifier of the blog post.</param>
    /// <returns>A <see cref="PostSummaryResponse"/> with post metadata and navigation links, or <see langword="null"/>.</returns>
    public async Task<PostSummaryResponse?> GetPostSummaryAsync(string postId)
    {
        var photos = await _db.Photos
            .Where(p => p.PostId == postId)
            .OrderBy(p => p.SortIndex ?? int.MaxValue)
            .ToListAsync();

        if (photos.Count == 0)
            return null;

        var first = photos.First();

        return new PostSummaryResponse
        {
            PostId = postId,
            PostUrl = first.PostUrl,
            PhotoCount = photos.Count,
            Links = new Dictionary<string, ApiLink>
            {
                ["self"] = new()
                {
                    Href = $"/posts/{Uri.EscapeDataString(postId)}"
                },
                ["photos"] = new()
                {
                    Href = $"/posts/{Uri.EscapeDataString(postId)}/photos"
                }
            }
        };
    }

    /// <summary>
    /// Returns the <see cref="PhotoDto"/> for the photo with the given slug, or <see langword="null"/> if not found.
    /// </summary>
    /// <param name="slug">The unique slug of the photo.</param>
    /// <returns>A <see cref="PhotoDto"/>, or <see langword="null"/> if no matching photo exists.</returns>
    public async Task<PhotoDto?> GetBySlugAsync(string slug)
    {
        var photo = await _db.Photos
            .FirstOrDefaultAsync(p => p.Slug == slug);

        return photo is null ? null : PhotoDtoMapper.ToDto(photo);
    }

    /// <summary>
    /// Returns a paged list of photos belonging to the specified gallery.
    /// Delegates to <see cref="GetPhotosByQueryAsync"/> so that all collection endpoints share the same filtering path.
    /// </summary>
    /// <param name="gallery">The gallery name.</param>
    /// <param name="page">The 1-based page number to retrieve. Defaults to 1.</param>
    /// <param name="pageSize">The maximum number of results per page. Defaults to 50.</param>
    /// <returns>A <see cref="PagedResponse{T}"/> of <see cref="PhotoDto"/> items for the gallery.</returns>
    public Task<PagedResponse<PhotoDto>> GetByGalleryAsync(string gallery, int page = 1, int pageSize = 50)
    {
        var options = new PhotoQueryOptions { Gallery = gallery, Page = page, PageSize = pageSize };
        return GetPhotosByQueryAsync(options, $"/galleries/{Uri.EscapeDataString(gallery)}/photos");
    }

    /// <summary>
    /// Returns a paged list of photos associated with the specified blog post.
    /// Delegates to <see cref="GetPhotosByQueryAsync"/> so that all collection endpoints share the same filtering path.
    /// </summary>
    /// <param name="postId">The blog post identifier.</param>
    /// <param name="page">The 1-based page number to retrieve. Defaults to 1.</param>
    /// <param name="pageSize">The maximum number of results per page. Defaults to 50.</param>
    /// <returns>A <see cref="PagedResponse{T}"/> of <see cref="PhotoDto"/> items for the post.</returns>
    public Task<PagedResponse<PhotoDto>> GetByPostAsync(string postId, int page = 1, int pageSize = 50)
    {
        var options = new PhotoQueryOptions { PostId = postId, Page = page, PageSize = pageSize };
        return GetPhotosByQueryAsync(options, $"/posts/{Uri.EscapeDataString(postId)}/photos");
    }

    /// <summary>
    /// Returns up to 500 photos taken on the given month and day across all years, ordered by most recent year first.
    /// </summary>
    /// <param name="month">The month (1–12).</param>
    /// <param name="day">The day of the month (1–31).</param>
    /// <returns>A flat list of <see cref="PhotoDto"/> items.</returns>
    public async Task<List<PhotoDto>> GetOnThisDayAsync(int month, int day)
    {
        var photos = await _db.Photos
            .Where(p => p.Month == month && p.Day == day)
            .OrderByDescending(p => p.Year)
            .ThenBy(p => p.SortIndex ?? int.MaxValue)
            .ThenBy(p => p.TakenAt)
            .Take(500)
            .ToListAsync();

        return photos.Select(PhotoDtoMapper.ToDto).ToList();
    }

    /// <summary>
    /// Returns an <see cref="OnThisDayResponse"/> that groups photos taken on the given month/day by year.
    /// </summary>
    /// <param name="month">The month (1–12).</param>
    /// <param name="day">The day of the month (1–31).</param>
    /// <returns>An <see cref="OnThisDayResponse"/> with photos grouped by year, most recent first.</returns>
    public async Task<OnThisDayResponse> GetOnThisDayGroupedAsync(int month, int day)
    {
        var photos = await GetOnThisDayAsync(month, day);

        return new OnThisDayResponse
        {
            Month = month,
            Day = day,
            Years = photos
                .Where(p => p.Year.HasValue)
                .GroupBy(p => p.Year!.Value)
                .OrderByDescending(g => g.Key)
                .Select(g => new OnThisDayYearGroup
                {
                    Year = g.Key,
                    Photos = g
                        .OrderBy(p => p.SortIndex ?? int.MaxValue)
                        .ThenBy(p => p.TakenAt)
                        .ToList()
                })
                .ToList()
        };
    }

    /// <summary>
    /// Returns the list of years that have at least one photo in the archive.
    /// </summary>
    /// <returns>A list of <see cref="YearSummaryDto"/> items ordered from most recent year to oldest.</returns>
    public async Task<List<YearSummaryDto>> GetYearsAsync()
    {
        var rows = await _db.Photos
            .Where(p => p.Year.HasValue)
            .GroupBy(p => p.Year!.Value)
            .Select(g => new
            {
                Year = g.Key,
                PhotoCount = g.Count()
            })
            .OrderByDescending(x => x.Year)
            .ToListAsync();

        return rows.Select(x => new YearSummaryDto
        {
            Year = x.Year,
            PhotoCount = x.PhotoCount,
            Links = new Dictionary<string, ApiLink>
            {
                ["months"] = new() { Href = $"/years/{x.Year}/months" },
                ["photos"] = new() { Href = $"/years/{x.Year}/photos" },
                ["query"] = new() { Href = $"/photos?year={x.Year}" }
            }
        }).ToList();
    }

    /// <summary>
    /// Returns the detail for the given year, including its months and navigation links, or <see langword="null"/> if no photos exist for that year.
    /// </summary>
    /// <param name="year">The four-digit year.</param>
    /// <returns>A <see cref="YearDetailResponse"/>, or <see langword="null"/>.</returns>
    public async Task<YearDetailResponse?> GetYearAsync(int year)
    {
        var months = await GetMonthsAsync(year);

        if (months.Count == 0)
            return null;

        return new YearDetailResponse
        {
            Year = year,
            PhotoCount = months.Sum(m => m.PhotoCount),
            Months = months,
            Links = new Dictionary<string, ApiLink>
            {
                ["self"] = new()
                {
                    Href = $"/years/{year}"
                },
                ["months"] = new()
                {
                    Href = $"/years/{year}/months"
                },
                ["photos"] = new()
                {
                    Href = $"/years/{year}/photos"
                },
                ["query"] = new()
                {
                    Href = $"/photos?year={year}"
                }
            }
        };
    }

    /// <summary>
    /// Returns the list of months within the given year that have at least one photo.
    /// </summary>
    /// <param name="year">The four-digit year.</param>
    /// <returns>A list of <see cref="MonthSummaryDto"/> items ordered by month number.</returns>
    public async Task<List<MonthSummaryDto>> GetMonthsAsync(int year)
    {
        var rows = await _db.Photos
            .Where(p => p.Year == year && p.Month.HasValue)
            .GroupBy(p => p.Month!.Value)
            .Select(g => new
            {
                Month = g.Key,
                PhotoCount = g.Count()
            })
            .OrderBy(x => x.Month)
            .ToListAsync();

        return rows.Select(x => new MonthSummaryDto
        {
            Year = year,
            Month = x.Month,
            PhotoCount = x.PhotoCount,
            Links = new Dictionary<string, ApiLink>
            {
                ["self"] = new()
                {
                    Href = $"/years/{year}/months/{x.Month}"
                },
                ["days"] = new()
                {
                    Href = $"/years/{year}/months/{x.Month}/days"
                },
                ["photos"] = new()
                {
                    Href = $"/years/{year}/months/{x.Month}/photos"
                },
                ["query"] = new()
                {
                    Href = $"/photos?year={year}&month={x.Month}"
                }
            }
        }).ToList();
    }

    /// <summary>
    /// Returns the detail for a specific year/month, including its days and navigation links, or <see langword="null"/> if no photos exist.
    /// </summary>
    /// <param name="year">The four-digit year.</param>
    /// <param name="month">The month number (1–12).</param>
    /// <returns>A <see cref="MonthDetailResponse"/>, or <see langword="null"/>.</returns>
    public async Task<MonthDetailResponse?> GetMonthAsync(int year, int month)
    {
        var days = await GetDaysAsync(year, month);

        if (days.Count == 0)
            return null;

        return new MonthDetailResponse
        {
            Year = year,
            Month = month,
            PhotoCount = days.Sum(d => d.PhotoCount),
            Days = days,
            Links = new Dictionary<string, ApiLink>
            {
                ["self"] = new()
                {
                    Href = $"/years/{year}/months/{month}"
                },
                ["days"] = new()
                {
                    Href = $"/years/{year}/months/{month}/days"
                },
                ["photos"] = new()
                {
                    Href = $"/years/{year}/months/{month}/photos"
                },
                ["query"] = new()
                {
                    Href = $"/photos?year={year}&month={month}"
                }
            }
        };
    }
    /// <summary>
    /// Returns the list of days within a given year/month that have at least one photo.
    /// </summary>
    /// <param name="year">The four-digit year.</param>
    /// <param name="month">The month number (1–12).</param>
    /// <returns>A list of <see cref="DaySummaryDto"/> items ordered by day number.</returns>
    public async Task<List<DaySummaryDto>> GetDaysAsync(int year, int month)
    {
        var rows = await _db.Photos
            .Where(p => p.Year == year && p.Month == month && p.Day.HasValue)
            .GroupBy(p => p.Day!.Value)
            .Select(g => new
            {
                Day = g.Key,
                PhotoCount = g.Count()
            })
            .OrderBy(x => x.Day)
            .ToListAsync();

        return rows.Select(x => new DaySummaryDto
        {
            Year = year,
            Month = month,
            Day = x.Day,
            PhotoCount = x.PhotoCount,
            Links = new Dictionary<string, ApiLink>
            {
                ["self"] = new()
                {
                    Href = $"/years/{year}/months/{month}/days/{x.Day}"
                },
                ["photos"] = new()
                {
                    Href = $"/years/{year}/months/{month}/days/{x.Day}/photos"
                },
                ["query"] = new()
                {
                    Href = $"/photos?year={year}&month={month}&day={x.Day}"
                },
                ["onThisDay"] = new()
                {
                    Href = $"/on-this-day?month={month}&day={x.Day}"
                }
            }
        }).ToList();
    }

    /// <summary>
    /// Returns the detail for a specific year/month/day, or <see langword="null"/> if no photos exist on that date.
    /// </summary>
    /// <param name="year">The four-digit year.</param>
    /// <param name="month">The month number (1–12).</param>
    /// <param name="day">The day of the month (1–31).</param>
    /// <returns>A <see cref="DayDetailResponse"/>, or <see langword="null"/>.</returns>
    public async Task<DayDetailResponse?> GetDayAsync(int year, int month, int day)
    {
        var photoCount = await _db.Photos
            .CountAsync(p =>
                p.Year == year &&
                p.Month == month &&
                p.Day == day);

        if (photoCount == 0)
            return null;

        return new DayDetailResponse
        {
            Year = year,
            Month = month,
            Day = day,
            PhotoCount = photoCount,
            Links = new Dictionary<string, ApiLink>
            {
                ["self"] = new()
                {
                    Href = $"/years/{year}/months/{month}/days/{day}"
                },
                ["photos"] = new()
                {
                    Href = $"/years/{year}/months/{month}/days/{day}/photos"
                },
                ["query"] = new()
                {
                    Href = $"/photos?year={year}&month={month}&day={day}"
                },
                ["onThisDay"] = new()
                {
                    Href = $"/on-this-day?month={month}&day={day}"
                }
            }
        };
    }

    /// <summary>
    /// Returns all photos taken on a specific date, ordered by sort index then taken time.
    /// </summary>
    /// <param name="year">The four-digit year.</param>
    /// <param name="month">The month number (1–12).</param>
    /// <param name="day">The day of the month (1–31).</param>
    /// <returns>A list of <see cref="PhotoDto"/> items for the specified date.</returns>
    public async Task<List<PhotoDto>> GetPhotosByDateAsync(int year, int month, int day)
    {
        var photos = await _db.Photos
            .Where(p => p.Year == year && p.Month == month && p.Day == day)
            .OrderBy(p => p.SortIndex ?? int.MaxValue)
            .ThenBy(p => p.TakenAt)
            .ToListAsync();

        return photos.Select(PhotoDtoMapper.ToDto).ToList();
    }
    /// <summary>
    /// Returns a paged list of photos taken in the specified year.
    /// Delegates to <see cref="GetPhotosByQueryAsync"/> so that all collection endpoints share the same filtering path.
    /// </summary>
    /// <param name="year">The four-digit year.</param>
    /// <param name="page">The 1-based page number to retrieve.</param>
    /// <param name="pageSize">The maximum number of results per page.</param>
    /// <returns>A <see cref="PagedResponse{T}"/> containing the matching <see cref="PhotoDto"/> items.</returns>
    public Task<PagedResponse<PhotoDto>> GetPhotosByYearAsync(int year, int page, int pageSize)
    {
        var options = new PhotoQueryOptions { Year = year, Page = page, PageSize = pageSize };
        return GetPhotosByQueryAsync(options, $"/years/{year}/photos");
    }

    /// <summary>
    /// Returns a paged list of photos taken in the specified year and month.
    /// Delegates to <see cref="GetPhotosByQueryAsync"/> so that all collection endpoints share the same filtering path.
    /// </summary>
    /// <param name="year">The four-digit year.</param>
    /// <param name="month">The month number (1–12).</param>
    /// <param name="page">The 1-based page number to retrieve.</param>
    /// <param name="pageSize">The maximum number of results per page.</param>
    /// <returns>A <see cref="PagedResponse{T}"/> containing the matching <see cref="PhotoDto"/> items.</returns>
    public Task<PagedResponse<PhotoDto>> GetPhotosByMonthAsync(int year, int month, int page, int pageSize)
    {
        var options = new PhotoQueryOptions { Year = year, Month = month, Page = page, PageSize = pageSize };
        return GetPhotosByQueryAsync(options, $"/years/{year}/months/{month}/photos");
    }

    /// <summary>
    /// Returns a paged list of photos taken on the specified year, month, and day.
    /// Delegates to <see cref="GetPhotosByQueryAsync"/> so that all collection endpoints share the same filtering path.
    /// </summary>
    /// <param name="year">The four-digit year.</param>
    /// <param name="month">The month number (1–12).</param>
    /// <param name="day">The day of the month (1–31).</param>
    /// <param name="page">The 1-based page number to retrieve.</param>
    /// <param name="pageSize">The maximum number of results per page.</param>
    /// <returns>A <see cref="PagedResponse{T}"/> containing the matching <see cref="PhotoDto"/> items.</returns>
    public Task<PagedResponse<PhotoDto>> GetPhotosByDayAsync(int year, int month, int day, int page, int pageSize)
    {
        var options = new PhotoQueryOptions { Year = year, Month = month, Day = day, Page = page, PageSize = pageSize };
        return GetPhotosByQueryAsync(options, $"/years/{year}/months/{month}/days/{day}/photos");
    }
}
