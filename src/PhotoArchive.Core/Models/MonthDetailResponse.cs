namespace PhotoArchive.Core.Models;

public class MonthDetailResponse
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int PhotoCount { get; set; }

    public List<DaySummaryDto> Days { get; set; } = [];

    public Dictionary<string, ApiLink> Links { get; set; } = [];
}