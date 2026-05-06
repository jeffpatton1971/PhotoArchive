namespace PhotoArchive.Core.Models;

/// <summary>
/// The response returned when listing days within a year/month that contain photos.
/// </summary>
public class DaysResponse
{
    /// <summary>Gets or sets the year.</summary>
    public int Year { get; set; }

    /// <summary>Gets or sets the month number (1–12).</summary>
    public int Month { get; set; }

    /// <summary>Gets or sets the total number of photos taken during this month.</summary>
    public int PhotoCount { get; set; }

    /// <summary>
    /// Gets or sets the hypermedia navigation links for this month's day listing.
    /// Includes: <c>self</c>, <c>photos</c>, <c>query</c>.
    /// </summary>
    public Dictionary<string, ApiLink> Links { get; set; } = [];

    /// <summary>Gets or sets the list of days within this month that contain at least one photo.</summary>
    public List<DaySummaryDto> Days { get; set; } = [];
}
