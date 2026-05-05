using Microsoft.AspNetCore.Mvc;
using PhotoArchive.Data.Services;

namespace PhotoArchive.Api.Controllers;

/// <summary>
/// Provides endpoints for querying individual photos, galleries, posts, and the "On This Day" feature.
/// </summary>
[ApiController]
[Route("photos")]
public class PhotosController : ControllerBase
{
    private readonly PhotoService _photoService;

    /// <summary>
    /// Initializes a new instance of <see cref="PhotosController"/>.
    /// </summary>
    /// <param name="photoService">The photo data service.</param>
    public PhotosController(PhotoService photoService)
    {
        _photoService = photoService;
    }

    /// <summary>
    /// Returns a paged list of photos, optionally filtered by year, month, and/or day.
    /// </summary>
    /// <param name="year">Optional year filter.</param>
    /// <param name="month">Optional month filter (1–12).</param>
    /// <param name="day">Optional day filter (1–31).</param>
    /// <param name="page">The 1-based page number. Defaults to 1.</param>
    /// <param name="pageSize">The number of results per page. Defaults to 50.</param>
    /// <returns>A paged list of photos matching the supplied filters.</returns>
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

    /// <summary>
    /// Returns photos grouped by year for a given month and day, defaulting to today's date if not specified.
    /// </summary>
    /// <param name="month">Optional month override (1–12). Defaults to the current UTC month.</param>
    /// <param name="day">Optional day override (1–31). Defaults to the current UTC day.</param>
    /// <returns>An <see cref="PhotoArchive.Core.Models.OnThisDayResponse"/> grouped by year.</returns>
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

    /// <summary>
    /// Returns the detail for a single photo identified by its slug.
    /// </summary>
    /// <param name="slug">The unique slug of the photo.</param>
    /// <returns>A <see cref="PhotoArchive.Core.Models.PhotoDetailResponse"/>, or 404 if not found.</returns>
    [HttpGet("/photos/{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var response = await _photoService.GetPhotoDetailAsync(slug);

        if (response is null)
            return NotFound();

        return Ok(response);
    }

    /// <summary>
    /// Returns all photos that belong to the specified gallery.
    /// </summary>
    /// <param name="gallery">The gallery name.</param>
    /// <returns>A list of photos in the gallery.</returns>
    [HttpGet("/galleries/{gallery}/photos")]
    public async Task<IActionResult> GetByGallery(string gallery)
    {
        var photos = await _photoService.GetByGalleryAsync(gallery);
        return Ok(photos);
    }

    /// <summary>
    /// Returns all photos associated with the specified blog post.
    /// </summary>
    /// <param name="postId">The blog post identifier.</param>
    /// <returns>A list of photos for the post.</returns>
    [HttpGet("/posts/{postId}/photos")]
    public async Task<IActionResult> GetByPost(string postId)
    {
        var photos = await _photoService.GetByPostAsync(postId);
        return Ok(photos);
    }

    /// <summary>
    /// Returns a summary for the specified blog post, including photo count and navigation links.
    /// </summary>
    /// <param name="postId">The blog post identifier.</param>
    /// <returns>A <see cref="PhotoArchive.Core.Models.PostSummaryResponse"/>, or 404 if no photos are associated with the post.</returns>
    [HttpGet("/posts/{postId}")]
    public async Task<IActionResult> GetPost(string postId)
    {
        var response = await _photoService.GetPostSummaryAsync(postId);

        if (response is null)
            return NotFound();

        return Ok(response);
    }
}