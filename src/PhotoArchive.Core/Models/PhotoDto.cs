namespace PhotoArchive.Core.Models;

/// <summary>
/// A lightweight data transfer object representing a photo in list and summary contexts.
/// </summary>
public class PhotoDto
{
    /// <summary>Gets or sets the URL-safe unique slug that identifies the photo.</summary>
    public string Slug { get; set; } = default!;

    /// <summary>Gets or sets the optional display title of the photo.</summary>
    public string? Title { get; set; }

    /// <summary>Gets or sets the date and time the photo was taken.</summary>
    public DateTimeOffset? TakenAt { get; set; }

    /// <summary>Gets or sets the year the photo was taken.</summary>
    public int? Year { get; set; }

    /// <summary>Gets or sets the month the photo was taken.</summary>
    public int? Month { get; set; }

    /// <summary>Gets or sets the day of the month the photo was taken.</summary>
    public int? Day { get; set; }

    /// <summary>Gets or sets the gallery name this photo belongs to, if any.</summary>
    public string? Gallery { get; set; }

    /// <summary>Gets or sets the URL of the associated blog post, if any.</summary>
    public string? PostUrl { get; set; }

    /// <summary>Gets or sets the identifier of the associated blog post, if any.</summary>
    public string? PostId { get; set; }

    /// <summary>Gets or sets the sort position within a gallery or post.</summary>
    public int? SortIndex { get; set; }

    /// <summary>Gets or sets the import source identifier (e.g., "legacy", "facebook").</summary>
    public string Source { get; set; } = default!;

    /// <summary>Gets or sets the URL to the full-resolution image.</summary>
    public string OriginalUrl { get; set; } = default!;

    /// <summary>Gets or sets the URL to the thumbnail image, if available.</summary>
    public string? ThumbUrl { get; set; }
}