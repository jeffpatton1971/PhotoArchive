namespace PhotoArchive.Core.Models;

/// <summary>
/// The response returned by the "On This Day" endpoint, grouping photos by year for a given month/day.
/// </summary>
public class OnThisDayResponse
{
    /// <summary>Gets or sets the month of the query (1–12).</summary>
    public int Month { get; set; }

    /// <summary>Gets or sets the day of the month of the query (1–31).</summary>
    public int Day { get; set; }

    /// <summary>Gets or sets the list of year groups, ordered from most recent to oldest.</summary>
    public List<OnThisDayYearGroup> Years { get; set; } = [];
}

/// <summary>
/// A group of photos taken on the same month/day in a specific year.
/// </summary>
public class OnThisDayYearGroup
{
    /// <summary>Gets or sets the year this group represents.</summary>
    public int Year { get; set; }

    /// <summary>Gets the number of photos in this year group.</summary>
    public int Count => Photos.Count;

    /// <summary>Gets or sets the photos taken on this day in this year.</summary>
    public List<PhotoDto> Photos { get; set; } = [];
}