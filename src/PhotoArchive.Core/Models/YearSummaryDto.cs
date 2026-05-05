namespace PhotoArchive.Core.Models;

public class YearSummaryDto
{
    public int Year { get; set; }
    public int PhotoCount { get; set; }
    public Dictionary<string, ApiLink> Links { get; set; } = [];
}
