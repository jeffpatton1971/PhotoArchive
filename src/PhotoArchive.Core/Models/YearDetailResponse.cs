namespace PhotoArchive.Core.Models;

/// <summary>
/// The detail response for a specific calendar year, including its months and navigation links.
/// </summary>
public class YearDetailResponse
{
    /// <summary>Gets or sets the year.</summary>
    public int Year { get; set; }

    /// <summary>Gets or sets the total number of photos taken during this year.</summary>
    public int PhotoCount { get; set; }

    /// <summary>Gets or sets the summary list of months within this year that contain photos.</summary>
    public List<MonthSummaryDto> Months { get; set; } = [];

    /// <summary>
    /// Gets or sets the hypermedia navigation links for this year.
    /// Includes: <c>self</c>, <c>months</c>, <c>photos</c>, <c>query</c>.
    /// </summary>
    public Dictionary<string, ApiLink> Links { get; set; } = [];
}