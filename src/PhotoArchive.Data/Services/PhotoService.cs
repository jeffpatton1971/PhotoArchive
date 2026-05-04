using Microsoft.EntityFrameworkCore;
using PhotoArchive.Core.Entities;

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
}