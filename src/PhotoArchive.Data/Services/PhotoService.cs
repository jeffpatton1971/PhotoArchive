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
        var query = _db.Photos.AsQueryable();

        if (year.HasValue)
            query = query.Where(p => p.Year == year);

        if (month.HasValue)
            query = query.Where(p => p.Month == month);

        if (day.HasValue)
            query = query.Where(p => p.Day == day);

        return await query
            .OrderByDescending(p => p.TakenAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => PhotoDtoMapper.ToDto(p))
            .ToListAsync();
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