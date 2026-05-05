using Microsoft.EntityFrameworkCore;
using PhotoArchive.Core.Entities;
using PhotoArchive.Core.Models;
using PhotoArchive.Core.Mapping;

namespace PhotoArchive.Data.Services;

public class PhotoService
{
    private readonly PhotoDbContext _db;

    public PhotoService(PhotoDbContext db)
    {
        _db = db;
    }

    private static string BuildPhotoPath(int? year, int? month, int? day)
    {
        var parameters = new List<string>();

        if (year.HasValue)
            parameters.Add($"year={year.Value}");

        if (month.HasValue)
            parameters.Add($"month={month.Value}");

        if (day.HasValue)
            parameters.Add($"day={day.Value}");

        if (parameters.Count == 0)
            return "/photos";

        return "/photos?" + string.Join("&", parameters);
    }

    private static string BuildPagedHref(string path, int page, int pageSize)
    {
        var separator = path.Contains('?') ? '&' : '?';
        return $"{path}{separator}page={page}&pageSize={pageSize}";
    }

    private async Task<PagedResponse<PhotoDto>> GetPagedPhotosAsync(
        IQueryable<Photo> query,
        string path,
        int page,
        int pageSize)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 250);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var photos = await query
            .OrderByDescending(p => p.TakenAt)
            .ThenBy(p => p.SortIndex ?? int.MaxValue)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var links = new Dictionary<string, ApiLink>
        {
            ["self"] = new() { Href = BuildPagedHref(path, page, pageSize) },
            ["first"] = new() { Href = BuildPagedHref(path, 1, pageSize) }
        };

        if (page > 1)
            links["previous"] = new() { Href = BuildPagedHref(path, page - 1, pageSize) };

        if (page < totalPages)
            links["next"] = new() { Href = BuildPagedHref(path, page + 1, pageSize) };

        if (totalPages > 0)
            links["last"] = new() { Href = BuildPagedHref(path, totalPages, pageSize) };

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

    public async Task<PagedResponse<PhotoDto>> GetPhotosAsync(
        int? year,
        int? month,
        int? day,
        int page = 1,
        int pageSize = 50)
    {
        var query = _db.Photos.AsQueryable();

        if (year.HasValue)
            query = query.Where(p => p.Year == year.Value);

        if (month.HasValue)
            query = query.Where(p => p.Month == month.Value);

        if (day.HasValue)
            query = query.Where(p => p.Day == day.Value);

        return await GetPagedPhotosAsync(query, BuildPhotoPath(year, month, day), page, pageSize);
    }

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
    public async Task<PhotoDto?> GetBySlugAsync(string slug)
    {
        var photo = await _db.Photos
            .FirstOrDefaultAsync(p => p.Slug == slug);

        return photo is null ? null : PhotoDtoMapper.ToDto(photo);
    }

    public async Task<List<PhotoDto>> GetByGalleryAsync(string gallery)
    {
        var photos = await _db.Photos
            .Where(p => p.Gallery == gallery)
            .OrderBy(p => p.SortIndex ?? int.MaxValue)
            .ThenBy(p => p.TakenAt)
            .ToListAsync();

        return photos.Select(PhotoDtoMapper.ToDto).ToList();
    }

    public async Task<List<PhotoDto>> GetByPostAsync(string postId)
    {
        var photos = await _db.Photos
            .Where(p => p.PostId == postId)
            .OrderBy(p => p.SortIndex ?? int.MaxValue)
            .ThenBy(p => p.TakenAt)
            .ToListAsync();

        return photos.Select(PhotoDtoMapper.ToDto).ToList();
    }

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
    public async Task<List<PhotoDto>> GetPhotosByDateAsync(int year, int month, int day)
    {
        var photos = await _db.Photos
            .Where(p => p.Year == year && p.Month == month && p.Day == day)
            .OrderBy(p => p.SortIndex ?? int.MaxValue)
            .ThenBy(p => p.TakenAt)
            .ToListAsync();

        return photos.Select(PhotoDtoMapper.ToDto).ToList();
    }
    public Task<PagedResponse<PhotoDto>> GetPhotosByYearAsync(int year, int page, int pageSize)
    {
        var query = _db.Photos.Where(p => p.Year == year);
        return GetPagedPhotosAsync(query, $"/years/{year}/photos", page, pageSize);
    }

    public Task<PagedResponse<PhotoDto>> GetPhotosByMonthAsync(int year, int month, int page, int pageSize)
    {
        var query = _db.Photos.Where(p => p.Year == year && p.Month == month);
        return GetPagedPhotosAsync(query, $"/years/{year}/months/{month}/photos", page, pageSize);
    }

    public Task<PagedResponse<PhotoDto>> GetPhotosByDayAsync(int year, int month, int day, int page, int pageSize)
    {
        var query = _db.Photos.Where(p => p.Year == year && p.Month == month && p.Day == day);
        return GetPagedPhotosAsync(query, $"/years/{year}/months/{month}/days/{day}/photos", page, pageSize);
    }
}
