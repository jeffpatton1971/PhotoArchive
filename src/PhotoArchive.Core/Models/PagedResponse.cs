namespace PhotoArchive.Core.Models;

/// <summary>
/// A generic paginated response wrapper returned by collection endpoints.
/// </summary>
/// <typeparam name="T">The type of items in the page.</typeparam>
public class PagedResponse<T>
{
    /// <summary>Gets or sets the current page number (1-based).</summary>
    public int Page { get; set; }

    /// <summary>Gets or sets the maximum number of items per page.</summary>
    public int PageSize { get; set; }

    /// <summary>Gets or sets the total number of matching items across all pages.</summary>
    public int TotalCount { get; set; }

    /// <summary>Gets or sets the total number of pages available.</summary>
    public int TotalPages { get; set; }

    /// <summary>Gets or sets the items on the current page.</summary>
    public List<T> Items { get; set; } = [];

    /// <summary>
    /// Gets or sets the hypermedia navigation links for the paged result.
    /// Common keys: <c>self</c>, <c>first</c>, <c>last</c>, <c>previous</c>, <c>next</c>.
    /// </summary>
    public Dictionary<string, ApiLink> Links { get; set; } = [];
}