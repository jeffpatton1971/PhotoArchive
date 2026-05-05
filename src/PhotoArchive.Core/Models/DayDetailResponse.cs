namespace PhotoArchive.Core.Models;

/// <summary>
/// The detail response for a specific calendar day, including photo count and navigation links.
/// </summary>
public class DayDetailResponse
{
    /// <summary>Gets or sets the year component of the date.</summary>
    public int Year { get; set; }

    /// <summary>Gets or sets the month component of the date (1–12).</summary>
    public int Month { get; set; }

    /// <summary>Gets or sets the day component of the date (1–31).</summary>
    public int Day { get; set; }

    /// <summary>Gets or sets the number of photos taken on this day.</summary>
    public int PhotoCount { get; set; }

    /// <summary>
    /// Gets or sets the hypermedia navigation links for this day.
    /// Includes: <c>self</c>, <c>photos</c>, <c>query</c>, <c>onThisDay</c>.
    /// </summary>
    public Dictionary<string, ApiLink> Links { get; set; } = [];
}