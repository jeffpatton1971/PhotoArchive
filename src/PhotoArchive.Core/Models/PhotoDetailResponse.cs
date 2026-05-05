namespace PhotoArchive.Core.Models;

/// <summary>
/// The full detail response returned for a single photo, including hypermedia links.
/// </summary>
public class PhotoDetailResponse
{
    /// <summary>Gets or sets the photo data transfer object.</summary>
    public PhotoDto Photo { get; set; } = default!;

    /// <summary>
    /// Gets or sets the hypermedia navigation links for this photo.
    /// May include: <c>self</c>, <c>sameDate</c>, <c>onThisDay</c>, <c>gallery</c>, <c>galleryPhotos</c>, <c>post</c>, <c>postPhotos</c>.
    /// </summary>
    public Dictionary<string, ApiLink> Links { get; set; } = [];
}