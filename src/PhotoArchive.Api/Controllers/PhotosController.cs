using Microsoft.AspNetCore.Mvc;
using PhotoArchive.Data.Services;
using PhotoArchive.Core.Models;

namespace PhotoArchive.Api.Controllers;

/// <summary>
/// Provides endpoints for querying the photo collection and retrieving individual photo detail.
/// </summary>
[ApiController]
[Route("photos")]
[Produces("application/json")]
[Tags("Photos")]
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
    /// <remarks>
    /// Pagination is 1-based: <paramref name="page"/> starts at 1 and <paramref name="pageSize"/> controls the number
    /// of items per page. Pagination links in the response (<c>self</c>, <c>first</c>, <c>last</c>,
    /// <c>previous</c>, <c>next</c>) preserve all active filter parameters.
    /// </remarks>
    /// <param name="year">Filter by the year the photo was taken.</param>
    /// <param name="month">Filter by the month the photo was taken (1–12).</param>
    /// <param name="day">Filter by the day the photo was taken (1–31).</param>
    /// <param name="source">Filter by import source identifier (e.g., "instagram", "facebook", "manual").</param>
    /// <param name="gallery">Filter by imported gallery identifier.</param>
    /// <param name="postId">Filter by associated legacy blog post identifier.</param>
    /// <param name="page">The 1-based page number. Defaults to 1.</param>
    /// <param name="pageSize">The number of results per page. Defaults to 50.</param>
    /// <returns>A paged list of photos matching the supplied filters.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<PhotoDto>), StatusCodes.Status200OK)]
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
    /// <param name="slug">The URL-safe unique identifier of the photo.</param>
    /// <returns>A <see cref="PhotoDetailResponse"/>, or 404 if not found.</returns>
    [HttpGet("/photos/{slug}")]
    [ProducesResponseType(typeof(PhotoDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var response = await _photoService.GetPhotoDetailAsync(slug);

        if (response is null)
            return NotFound();

        return Ok(response);
    }
}