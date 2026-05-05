namespace PhotoArchive.Core.Models;

public class PostSummaryResponse
{
    public string PostId { get; set; } = default!;
    public string? PostUrl { get; set; }
    public int PhotoCount { get; set; }

    public Dictionary<string, ApiLink> Links { get; set; } = [];
}