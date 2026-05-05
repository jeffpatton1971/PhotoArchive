using Microsoft.EntityFrameworkCore;
using PhotoArchive.Core.Entities;
using PhotoArchive.Core.Models;

namespace PhotoArchive.Data.Services;

public class PhotoService
{
    private readonly PhotoDbContext _db;

    public PhotoService(PhotoDbContext db)
    {
        _db = db;
    }

    public async Task<List<Photo>> GetPhotosAsync(int? year, int? month, int? day)
    {
        var query = _db.Photos.AsQueryable();

        if (year.HasValue)
            query = query.Where(p => p.Year == year.Value);

        if (month.HasValue)
            query = query.Where(p => p.Month == month.Value);

        if (day.HasValue)
            query = query.Where(p => p.Day == day.Value);

        return await query
            .OrderByDescending(p => p.TakenAt)
            .Take(100)
            .ToListAsync();
    }
    public async Task<List<Photo>> GetOnThisDayAsync(int month, int day)
    {
        return await _db.Photos
            .Where(p => p.Month == month && p.Day == day)
            .OrderByDescending(p => p.Year)
            .ThenByDescending(p => p.TakenAt)
            .Take(500)
            .ToListAsync();
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