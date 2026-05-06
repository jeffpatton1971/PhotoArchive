namespace PhotoArchive.Core.Models;

/// <summary>
/// The response returned by the archive index endpoint, listing all years that contain photos.
/// </summary>
public class YearsResponse
{
    /// <summary>Gets or sets the list of years that contain at least one photo.</summary>
    public List<YearSummaryDto> Years { get; set; } = [];
}
