namespace PhotoArchive.Core.Models;

public class YearDetailResponse
{
    public int Year { get; set; }
    public int PhotoCount { get; set; }
    public List<MonthSummaryDto> Months { get; set; } = [];
    public Dictionary<string, ApiLink> Links { get; set; } = [];
}