namespace PhotoArchive.Core.Models;

public class DayDetailResponse
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int Day { get; set; }
    public int PhotoCount { get; set; }

    public Dictionary<string, ApiLink> Links { get; set; } = [];
}