namespace PhotoArchive.Core.Models;

/// <summary>
/// Builds canonical resource link strings for PhotoArchive API responses.
/// All route strings produced here must match the routes declared in the API controllers.
/// </summary>
/// <remarks>
/// Methods return raw path strings (with optional query parameters) that are suitable
/// for use as the <see cref="ApiLink.Href"/> value. They do not perform HTTP dispatch
/// and have no dependency on ASP.NET routing infrastructure.
/// </remarks>
public static class ResourceLinkBuilder
{
    // ── Photos ────────────────────────────────────────────────────────────────

    /// <summary>Returns the path for the photos collection: <c>/photos</c>.</summary>
    public static string Photos() => "/photos";

    /// <summary>Returns the self path for a single photo: <c>/photos/{slug}</c>.</summary>
    /// <param name="slug">The URL-safe photo slug.</param>
    public static string PhotoSelf(string slug) => $"/photos/{Uri.EscapeDataString(slug)}";

    /// <summary>
    /// Returns the <c>/photos</c> query path with any supplied filter parameters appended.
    /// Only non-null/non-empty values are included in the query string.
    /// </summary>
    /// <param name="year">Optional year filter.</param>
    /// <param name="month">Optional month filter (1–12).</param>
    /// <param name="day">Optional day filter (1–31).</param>
    /// <param name="source">Optional import source filter.</param>
    /// <param name="gallery">Optional gallery name filter.</param>
    /// <param name="postId">Optional blog post identifier filter.</param>
    public static string PhotosQuery(
        int? year = null,
        int? month = null,
        int? day = null,
        string? source = null,
        string? gallery = null,
        string? postId = null)
    {
        var parameters = new List<string>();

        if (year.HasValue)
            parameters.Add($"year={year.Value}");

        if (month.HasValue)
            parameters.Add($"month={month.Value}");

        if (day.HasValue)
            parameters.Add($"day={day.Value}");

        if (!string.IsNullOrWhiteSpace(source))
            parameters.Add($"source={Uri.EscapeDataString(source)}");

        if (!string.IsNullOrWhiteSpace(gallery))
            parameters.Add($"gallery={Uri.EscapeDataString(gallery)}");

        if (!string.IsNullOrWhiteSpace(postId))
            parameters.Add($"postId={Uri.EscapeDataString(postId)}");

        return parameters.Count == 0
            ? "/photos"
            : "/photos?" + string.Join("&", parameters);
    }

    // ── Years ─────────────────────────────────────────────────────────────────

    /// <summary>Returns the path for the years index: <c>/years</c>.</summary>
    public static string Years() => "/years";

    /// <summary>Returns the self path for a year: <c>/years/{year}</c>.</summary>
    /// <param name="year">The four-digit year.</param>
    public static string Year(int year) => $"/years/{year}";

    /// <summary>Returns the photos path for a year: <c>/years/{year}/photos</c>.</summary>
    /// <param name="year">The four-digit year.</param>
    public static string YearPhotos(int year) => $"/years/{year}/photos";

    /// <summary>Returns the months path for a year: <c>/years/{year}/months</c>.</summary>
    /// <param name="year">The four-digit year.</param>
    public static string YearMonths(int year) => $"/years/{year}/months";

    // ── Months ────────────────────────────────────────────────────────────────

    /// <summary>Returns the self path for a month: <c>/years/{year}/months/{month}</c>.</summary>
    /// <param name="year">The four-digit year.</param>
    /// <param name="month">The month number (1–12).</param>
    public static string Month(int year, int month) => $"/years/{year}/months/{month}";

    /// <summary>Returns the photos path for a month: <c>/years/{year}/months/{month}/photos</c>.</summary>
    /// <param name="year">The four-digit year.</param>
    /// <param name="month">The month number (1–12).</param>
    public static string MonthPhotos(int year, int month) => $"/years/{year}/months/{month}/photos";

    /// <summary>Returns the days path for a month: <c>/years/{year}/months/{month}/days</c>.</summary>
    /// <param name="year">The four-digit year.</param>
    /// <param name="month">The month number (1–12).</param>
    public static string MonthDays(int year, int month) => $"/years/{year}/months/{month}/days";

    // ── Days ──────────────────────────────────────────────────────────────────

    /// <summary>Returns the self path for a day: <c>/years/{year}/months/{month}/days/{day}</c>.</summary>
    /// <param name="year">The four-digit year.</param>
    /// <param name="month">The month number (1–12).</param>
    /// <param name="day">The day of the month (1–31).</param>
    public static string Day(int year, int month, int day) => $"/years/{year}/months/{month}/days/{day}";

    /// <summary>Returns the photos path for a day: <c>/years/{year}/months/{month}/days/{day}/photos</c>.</summary>
    /// <param name="year">The four-digit year.</param>
    /// <param name="month">The month number (1–12).</param>
    /// <param name="day">The day of the month (1–31).</param>
    public static string DayPhotos(int year, int month, int day) => $"/years/{year}/months/{month}/days/{day}/photos";

    // ── Posts ─────────────────────────────────────────────────────────────────

    /// <summary>Returns the self path for a post: <c>/posts/{postId}</c>.</summary>
    /// <param name="postId">The blog post identifier.</param>
    public static string Post(string postId) => $"/posts/{Uri.EscapeDataString(postId)}";

    /// <summary>Returns the photos path for a post: <c>/posts/{postId}/photos</c>.</summary>
    /// <param name="postId">The blog post identifier.</param>
    public static string PostPhotos(string postId) => $"/posts/{Uri.EscapeDataString(postId)}/photos";

    // ── Galleries ─────────────────────────────────────────────────────────────

    /// <summary>Returns the gallery resource path: <c>/galleries/{gallery}</c>.</summary>
    /// <param name="gallery">The imported gallery identifier.</param>
    public static string Gallery(string gallery) => $"/galleries/{Uri.EscapeDataString(gallery)}";

    /// <summary>Returns the photos path for a gallery: <c>/galleries/{gallery}/photos</c>.</summary>
    /// <param name="gallery">The imported gallery identifier.</param>
    public static string GalleryPhotos(string gallery) => $"/galleries/{Uri.EscapeDataString(gallery)}/photos";

    // ── On This Day ───────────────────────────────────────────────────────────

    /// <summary>Returns the on-this-day query path: <c>/on-this-day?month={month}&amp;day={day}</c>.</summary>
    /// <param name="month">The month number (1–12).</param>
    /// <param name="day">The day of the month (1–31).</param>
    public static string OnThisDay(int month, int day) => $"/on-this-day?month={month}&day={day}";
}
