using PhotoArchive.Core.Entities;

public static class PhotoMapper
{
    public static Photo Map(Dictionary<string, object> data)
    {
        return new Photo
        {
            Id = Guid.NewGuid(),
            Slug = data.TryGetValue("id", out var id) ? id?.ToString()! : Guid.NewGuid().ToString(),

            Title = data.TryGetValue("title", out var title) ? title?.ToString() : null,

            Source = data.TryGetValue("source", out var source)
                ? source.ToString() ?? "unknown"
                : "unknown",

            OriginalUrl = data.TryGetValue("raw_url", out var raw)
                ? raw?.ToString() ?? ""
                : "",

            ThumbUrl = data.TryGetValue("thumb_url", out var thumb)
                ? thumb?.ToString()
                : null
        };
    }
}