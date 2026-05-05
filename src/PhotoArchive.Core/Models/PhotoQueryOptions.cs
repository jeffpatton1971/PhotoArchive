namespace PhotoArchive.Core.Models;

/// <summary>
/// Represents the filter and pagination options used to query photo collections.
/// All photo collection endpoints share this query model to ensure consistent filtering behavior.
/// </summary>
public class PhotoQueryOptions
{
    /// <summary>Gets or sets the optional year filter.</summary>
    public int? Year { get; set; }

    /// <summary>Gets or sets the optional month filter (1–12).</summary>
    public int? Month { get; set; }

    /// <summary>Gets or sets the optional day filter (1–31).</summary>
    public int? Day { get; set; }

    /// <summary>Gets or sets the optional import source filter (e.g., "legacy", "facebook").</summary>
    public string? Source { get; set; }

    /// <summary>Gets or sets the optional gallery name filter.</summary>
    public string? Gallery { get; set; }

    /// <summary>Gets or sets the optional blog post identifier filter.</summary>
    public string? PostId { get; set; }

    /// <summary>Gets or sets the 1-based page number. Defaults to 1.</summary>
    public int Page { get; set; } = 1;

    /// <summary>Gets or sets the maximum number of results per page. Defaults to 50.</summary>
    public int PageSize { get; set; } = 50;
}
