using Microsoft.AspNetCore.Mvc;
using PhotoArchive.Data.Services;
using PhotoArchive.Core.Models;

namespace PhotoArchive.Api.Controllers;

/// <summary>
/// Provides endpoints for querying photos within a named gallery.
/// Gallery identity is derived from imported metadata; no first-class Gallery entity exists yet.
/// </summary>
[ApiController]
public class GalleriesController : ControllerBase
{
    private readonly PhotoService _photoService;

    /// <summary>
    /// Initializes a new instance of <see cref="GalleriesController"/>.
    /// </summary>
    /// <param name="photoService">The photo data service.</param>
    public GalleriesController(PhotoService photoService)
    {
        _photoService = photoService;
    }

    /// <summary>
    /// Returns a paged list of photos belonging to the specified gallery.
    /// </summary>
    /// <param name="gallery">The gallery name.</param>
    /// <param name="page">The 1-based page number. Defaults to 1.</param>
    /// <param name="pageSize">The number of results per page. Defaults to 50.</param>
    /// <returns>A <see cref="PagedResponse{T}"/> of <see cref="PhotoDto"/> items in the gallery.</returns>
    [HttpGet("/galleries/{gallery}/photos")]
    public async Task<IActionResult> GetByGallery(string gallery, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        var photos = await _photoService.GetByGalleryAsync(gallery, page, pageSize);
        return Ok(photos);
    }
}
