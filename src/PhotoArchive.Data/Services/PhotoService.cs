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

    public async Task<List<PhotoDto>> GetPhotosAsync(
        int? year,
        int? month,
        int? day,
        int page = 1,
        int pageSize = 50)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 250);

        var query = _db.Photos.AsQueryable();

        if (year.HasValue)
            query = query.Where(p => p.Year == year.Value);

        if (month.HasValue)
            query = query.Where(p => p.Month == month.Value);

        if (day.HasValue)
            query = query.Where(p => p.Day == day.Value);

        var photos = await query
            .OrderByDescending(p => p.TakenAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return photos.Select(PhotoDtoMapper.ToDto).ToList();
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
}