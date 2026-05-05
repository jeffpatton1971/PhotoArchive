namespace PhotoArchive.Core.Models;

public class PhotoDto
{
    public string Slug { get; set; } = default!;
    public string? Title { get; set; }

    public DateTimeOffset? TakenAt { get; set; }
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? Day { get; set; }

    public string? Gallery { get; set; }
    public string? PostUrl { get; set; }
    public string? PostId { get; set; }
    public int? SortIndex { get; set; }

    public string Source { get; set; } = default!;
    public string OriginalUrl { get; set; } = default!;
    public string? ThumbUrl { get; set; }
}