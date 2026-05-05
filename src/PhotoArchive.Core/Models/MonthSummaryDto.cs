namespace PhotoArchive.Core.Models;

/// <summary>
/// A summary data transfer object for a single calendar month within a year listing.
/// </summary>
public class MonthSummaryDto
{
    /// <summary>Gets or sets the year of the month.</summary>
    public int Year { get; set; }

    /// <summary>Gets or sets the month number (1–12).</summary>
    public int Month { get; set; }

    /// <summary>Gets or sets the total number of photos taken during this month.</summary>
    public int PhotoCount { get; set; }

    /// <summary>
    /// Gets or sets the hypermedia navigation links for this month.
    /// Includes: <c>self</c>, <c>days</c>, <c>photos</c>, <c>query</c>.
    /// </summary>
    public Dictionary<string, ApiLink> Links { get; set; } = [];
}
