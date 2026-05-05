using PhotoArchive.Core.Entities;
using PhotoArchive.Core.Models;

namespace PhotoArchive.Core.Mapping;

public static class PhotoDtoMapper
{
    public static PhotoDto ToDto(Photo photo) => new()
    {
        Slug = photo.Slug,
        Title = photo.Title,
        TakenAt = photo.TakenAt,
        Year = photo.Year,
        Month = photo.Month,
        Day = photo.Day,
        Gallery = photo.Gallery,
        PostUrl = photo.PostUrl,
        PostId = photo.PostId,
        SortIndex = photo.SortIndex,
        Source = photo.Source,
        OriginalUrl = photo.OriginalUrl,
        ThumbUrl = photo.ThumbUrl
    };
}