using PhotoArchive.Core.Entities;
using PhotoArchive.Core.Models;

namespace PhotoArchive.Core.Mapping;

public static class PhotoDtoMapper
{
    public static PhotoDto ToDto(Photo p) => new()
    {
        Slug = p.Slug,
        Title = p.Title,
        Year = p.Year,
        Month = p.Month,
        Day = p.Day,
        Gallery = p.Gallery,
        PostUrl = p.PostUrl,
        Source = p.Source,
        OriginalUrl = p.OriginalUrl,
        ThumbUrl = p.ThumbUrl
    };
}