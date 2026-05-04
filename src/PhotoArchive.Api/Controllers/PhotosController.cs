using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoArchive.Data;

namespace PhotoArchive.Api.Controllers;

[ApiController]
[Route("photos")]
public class PhotosController : ControllerBase
{
    private readonly PhotoDbContext _db;

    public PhotosController(PhotoDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetPhotos()
    {
        var photos = await _db.Photos
            .OrderByDescending(p => p.TakenAt)
            .Take(50)
            .ToListAsync();

        return Ok(photos);
    }
}