using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoArchive.Data;
using PhotoArchive.Data.Services;

namespace PhotoArchive.Api.Controllers;

[ApiController]
[Route("photos")]
public class PhotosController : ControllerBase
{
    private readonly PhotoDbContext _db;
    private readonly PhotoService _photoService;

    public PhotosController(PhotoDbContext db, PhotoService photoService)
    {
        _db = db;
        _photoService = photoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPhotos([FromQuery] int? year, [FromQuery] int? month, [FromQuery] int? day)
    {
        var photos = await _photoService.GetPhotosAsync(year, month, day);
        return Ok(photos);
    }
    [HttpGet("/on-this-day")]
    public async Task<IActionResult> GetOnThisDay([FromQuery] int? month, [FromQuery] int? day)
    {
        var targetDate = DateTime.UtcNow;

        var targetMonth = month ?? targetDate.Month;
        var targetDay = day ?? targetDate.Day;

        if (targetMonth < 1 || targetMonth > 12)
            return BadRequest("Month must be between 1 and 12.");

        if (targetDay < 1 || targetDay > 31)
            return BadRequest("Day must be between 1 and 31.");

        var photos = await _photoService.GetOnThisDayAsync(targetMonth, targetDay);

        return Ok(new
        {
            month = targetMonth,
            day = targetDay,
            photos
        });
    }
    [HttpGet("/photos/{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var photo = await _db.Photos
            .FirstOrDefaultAsync(p => p.Slug == slug);

        if (photo == null)
            return NotFound();

        return Ok(photo);
    }
}