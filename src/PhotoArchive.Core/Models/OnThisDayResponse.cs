namespace PhotoArchive.Core.Models;

public class OnThisDayResponse
{
    public int Month { get; set; }
    public int Day { get; set; }
    public List<OnThisDayYearGroup> Years { get; set; } = [];
}

public class OnThisDayYearGroup
{
    public int Year { get; set; }
    public int Count => Photos.Count;
    public List<PhotoDto> Photos { get; set; } = [];
}