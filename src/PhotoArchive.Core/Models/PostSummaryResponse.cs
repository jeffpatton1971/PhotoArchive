namespace PhotoArchive.Core.Models;

/// <summary>
/// The response returned for a blog post, summarising the photos associated with it.
/// </summary>
public class PostSummaryResponse
{
    /// <summary>Gets or sets the identifier of the blog post.</summary>
    public string PostId { get; set; } = default!;

    /// <summary>Gets or sets the URL of the blog post, if available.</summary>
    public string? PostUrl { get; set; }

    /// <summary>Gets or sets the total number of photos associated with this post.</summary>
    public int PhotoCount { get; set; }

    /// <summary>
    /// Gets or sets the hypermedia navigation links for this post.
    /// Includes: <c>self</c>, <c>photos</c>.
    /// </summary>
    public Dictionary<string, ApiLink> Links { get; set; } = [];
}