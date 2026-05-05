using Microsoft.AspNetCore.Mvc;
using PhotoArchive.Data.Services;

namespace PhotoArchive.Api.Controllers;

[ApiController]
[Route("photos")]
public class PhotosController : ControllerBase
{
    private readonly PhotoService _photoService;

    public PhotosController(PhotoService photoService)
    {
        _photoService = photoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPhotos(
        [FromQuery] int? year,
        [FromQuery] int? month,
        [FromQuery] int? day,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        var photos = await _photoService.GetPhotosAsync(year, month, day, page, pageSize);
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

        var response = await _photoService.GetOnThisDayGroupedAsync(targetMonth, targetDay);

        return Ok(response);
    }
    [HttpGet("/photos/{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var response = await _photoService.GetPhotoDetailAsync(slug);

        if (response is null)
            return NotFound();

        return Ok(response);
    }

    [HttpGet("/galleries/{gallery}/photos")]
    public async Task<IActionResult> GetByGallery(string gallery)
    {
        var photos = await _photoService.GetByGalleryAsync(gallery);
        return Ok(photos);
    }

    [HttpGet("/posts/{postId}/photos")]
    public async Task<IActionResult> GetByPost(string postId)
    {
        var photos = await _photoService.GetByPostAsync(postId);
        return Ok(photos);
    }
    [HttpGet("/posts/{postId}")]
    public async Task<IActionResult> GetPost(string postId)
    {
        var response = await _photoService.GetPostSummaryAsync(postId);

        if (response is null)
            return NotFound();

        return Ok(response);
    }
}