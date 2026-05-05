using System.Text.Json;
using PhotoArchive.Core.Entities;

public static class PhotoMapper
{
    public static Photo Map(Dictionary<string, object> data)
    {
        var id = GetString(data, "id");

        if (string.IsNullOrWhiteSpace(id))
            throw new InvalidOperationException("Photo front matter is missing required field: id");

        var takenAt = GetDate(data, "taken_at");

        return new Photo
        {
            Id = Guid.NewGuid(),
            Slug = id,

            Title = GetString(data, "title"),
            TakenAt = takenAt,

            Year = GetInt(data, "year") ?? takenAt?.Year,
            Month = GetInt(data, "month") ?? takenAt?.Month,
            Day = GetInt(data, "day") ?? takenAt?.Day,

            Source = GetSource(data),
            OriginalUrl = GetString(data, "raw_url") ?? "",
            ThumbUrl = GetString(data, "thumb_url"),

            // ADD THEM HERE
            Gallery = GetString(data, "gallery"),
            PostUrl = GetString(data, "post"),
            PostId = GetString(data, "post_id"),
            SortIndex = GetInt(data, "index"),
            SourceFilename = GetString(data, "source_filename"),

            // Always last
            SourceMetadataJson = JsonSerializer.Serialize(data)
        };
    }

    private static string GetSource(Dictionary<string, object> data)
    {
        if (!data.TryGetValue("source", out var source) || source is null)
            return "unknown";

        // Handles: source: legacy
        if (source is string sourceString)
            return string.IsNullOrWhiteSpace(sourceString) ? "unknown" : sourceString;

        // Handles:
        // source:
        //   type: facebook
        if (source is Dictionary<object, object> sourceDictionary)
        {
            if (sourceDictionary.TryGetValue("type", out var type) && type is not null)
                return type.ToString() ?? "unknown";
        }

        return source.ToString() ?? "unknown";
    }

    private static string? GetString(Dictionary<string, object> data, string key)
    {
        if (!data.TryGetValue(key, out var value) || value is null)
            return null;

        var text = value.ToString();
        return string.IsNullOrWhiteSpace(text) ? null : text;
    }

    private static int? GetInt(Dictionary<string, object> data, string key)
    {
        if (!data.TryGetValue(key, out var value) || value is null)
            return null;

        if (value is int intValue)
            return intValue;

        return int.TryParse(value.ToString(), out var parsed)
            ? parsed
            : null;
    }

    private static DateTimeOffset? GetDate(Dictionary<string, object> data, string key)
    {
        if (!data.TryGetValue(key, out var value) || value is null)
            return null;

        if (value is DateTime dateTime)
        {
            var unspecified = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            return new DateTimeOffset(unspecified);
        }
        return DateTimeOffset.TryParse(value.ToString(), out var parsed)
            ? parsed.ToUniversalTime()
            : null;
    }
}