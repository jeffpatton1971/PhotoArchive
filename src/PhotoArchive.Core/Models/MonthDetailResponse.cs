namespace PhotoArchive.Core.Models;

/// <summary>
/// The detail response for a specific calendar month, including its days and navigation links.
/// </summary>
public class MonthDetailResponse
{
    /// <summary>Gets or sets the year of the month.</summary>
    public int Year { get; set; }

    /// <summary>Gets or sets the month number (1–12).</summary>
    public int Month { get; set; }

    /// <summary>Gets or sets the total number of photos taken during this month.</summary>
    public int PhotoCount { get; set; }

    /// <summary>Gets or sets the summary list of days within this month that contain photos.</summary>
    public List<DaySummaryDto> Days { get; set; } = [];

    /// <summary>
    /// Gets or sets the hypermedia navigation links for this month.
    /// Includes: <c>self</c>, <c>days</c>, <c>photos</c>, <c>query</c>.
    /// </summary>
    public Dictionary<string, ApiLink> Links { get; set; } = [];
}