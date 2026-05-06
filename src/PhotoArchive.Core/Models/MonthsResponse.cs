namespace PhotoArchive.Core.Models;

/// <summary>
/// The response returned when listing months within a year that contain photos.
/// </summary>
public class MonthsResponse
{
    /// <summary>Gets or sets the year.</summary>
    public int Year { get; set; }

    /// <summary>Gets or sets the list of months within this year that contain at least one photo.</summary>
    public List<MonthSummaryDto> Months { get; set; } = [];
}
