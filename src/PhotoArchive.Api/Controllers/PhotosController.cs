using Microsoft.AspNetCore.Mvc;
using PhotoArchive.Data.Services;

namespace PhotoArchive.Api.Controllers;

/// <summary>
/// Provides endpoints for querying the photo collection and retrieving individual photo detail.
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
    /// Returns a paged list of photos, optionally filtered by year, month, day, source, gallery, and/or post.
    /// </summary>
    /// <param name="year">Optional year filter.</param>
    /// <param name="month">Optional month filter (1–12).</param>
    /// <param name="day">Optional day filter (1–31).</param>
    /// <param name="source">Optional import source filter (e.g., "legacy", "facebook").</param>
    /// <param name="gallery">Optional gallery name filter.</param>
    /// <param name="postId">Optional blog post identifier filter.</param>
    /// <param name="page">The 1-based page number. Defaults to 1.</param>
    /// <param name="pageSize">The number of results per page. Defaults to 50.</param>
    /// <returns>A paged list of photos matching the supplied filters.</returns>
    [HttpGet]
    public async Task<IActionResult> GetPhotos(
        [FromQuery] int? year,
        [FromQuery] int? month,
        [FromQuery] int? day,
        [FromQuery] string? source,
        [FromQuery] string? gallery,
        [FromQuery] string? postId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        var photos = await _photoService.GetPhotosAsync(year, month, day, page, pageSize, source, gallery, postId);
        return Ok(photos);
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
}