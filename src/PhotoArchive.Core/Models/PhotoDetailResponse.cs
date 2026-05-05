namespace PhotoArchive.Core.Models;

public class PhotoDetailResponse
{
    public PhotoDto Photo { get; set; } = default!;
    public Dictionary<string, ApiLink> Links { get; set; } = [];
}