using Microsoft.AspNetCore.Mvc;
using PhotoArchive.Data.Services;
using PhotoArchive.Core.Models;

namespace PhotoArchive.Api.Controllers;

/// <summary>
/// Provides endpoints for querying blog posts and their associated photos.
/// </summary>
[ApiController]
public class PostsController : ControllerBase
{
    private readonly PhotoService _photoService;

    /// <summary>
    /// Initializes a new instance of <see cref="PostsController"/>.
    /// </summary>
    /// <param name="photoService">The photo data service.</param>
    public PostsController(PhotoService photoService)
    {
        _photoService = photoService;
    }

    /// <summary>
    /// Returns a summary for the specified blog post, including photo count and navigation links.
    /// </summary>
    /// <param name="postId">The blog post identifier.</param>
    /// <returns>A <see cref="PostSummaryResponse"/>, or 404 if no photos are associated with the post.</returns>
    [HttpGet("/posts/{postId}")]
    public async Task<IActionResult> GetPost(string postId)
    {
        var response = await _photoService.GetPostSummaryAsync(postId);

        if (response is null)
            return NotFound();

        return Ok(response);
    }

    /// <summary>
    /// Returns a paged list of photos associated with the specified blog post.
    /// </summary>
    /// <param name="postId">The blog post identifier.</param>
    /// <param name="page">The 1-based page number. Defaults to 1.</param>
    /// <param name="pageSize">The number of results per page. Defaults to 50.</param>
    /// <returns>A <see cref="PagedResponse{T}"/> of <see cref="PhotoDto"/> items for the post.</returns>
    [HttpGet("/posts/{postId}/photos")]
    public async Task<IActionResult> GetByPost(string postId, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        var photos = await _photoService.GetByPostAsync(postId, page, pageSize);
        return Ok(photos);
    }
}
