namespace PhotoArchive.Core.Models;

/// <summary>
/// A summary data transfer object for a single calendar year in the archive index.
/// </summary>
public class YearSummaryDto
{
    /// <summary>Gets or sets the year.</summary>
    public int Year { get; set; }

    /// <summary>Gets or sets the total number of photos taken during this year.</summary>
    public int PhotoCount { get; set; }

    /// <summary>
    /// Gets or sets the hypermedia navigation links for this year.
    /// Includes: <c>months</c>, <c>photos</c>, <c>query</c>.
    /// </summary>
    public Dictionary<string, ApiLink> Links { get; set; } = [];
}
