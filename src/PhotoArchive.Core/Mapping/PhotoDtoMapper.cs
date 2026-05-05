using PhotoArchive.Core.Entities;
using PhotoArchive.Core.Models;

namespace PhotoArchive.Core.Mapping;

/// <summary>
/// Provides mapping from <see cref="Photo"/> entities to <see cref="PhotoDto"/> data transfer objects.
/// </summary>
public static class PhotoDtoMapper
{
    /// <summary>
    /// Maps a <see cref="Photo"/> entity to a <see cref="PhotoDto"/>.
    /// </summary>
    /// <param name="photo">The source photo entity.</param>
    /// <returns>A new <see cref="PhotoDto"/> populated from the entity.</returns>
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