namespace PhotoArchive.Core.Models;

/// <summary>
/// Builds hypermedia pagination links for paged collection responses.
/// Accepts a base path that already contains any active filter query parameters
/// (but not <c>page</c> or <c>pageSize</c>) and produces the standard self/first/previous/next/last
/// link dictionary expected by <see cref="PagedResponse{T}"/>.
/// </summary>
public static class PaginationLinkBuilder
{
    /// <summary>
    /// Builds the standard pagination link dictionary for a paged collection.
    /// </summary>
    /// <param name="basePath">
    /// The canonical base path including any active filter query parameters, but <em>excluding</em>
    /// <c>page</c> and <c>pageSize</c>.
    /// Examples: <c>/photos</c>, <c>/photos?source=instagram</c>, <c>/years/2022/photos</c>.
    /// </param>
    /// <param name="page">The current 1-based page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="totalPages">The total number of pages available.</param>
    /// <returns>
    /// A dictionary of named <see cref="ApiLink"/> entries.
    /// Always contains <c>self</c> and <c>first</c>.
    /// Contains <c>previous</c> when <paramref name="page"/> &gt; 1.
    /// Contains <c>next</c> when <paramref name="page"/> &lt; <paramref name="totalPages"/>.
    /// Contains <c>last</c> when <paramref name="totalPages"/> &gt; 0.
    /// </returns>
    public static Dictionary<string, ApiLink> Build(string basePath, int page, int pageSize, int totalPages)
    {
        var links = new Dictionary<string, ApiLink>
        {
            ["self"] = new() { Href = BuildHref(basePath, page, pageSize) },
            ["first"] = new() { Href = BuildHref(basePath, 1, pageSize) }
        };

        if (page > 1)
            links["previous"] = new() { Href = BuildHref(basePath, page - 1, pageSize) };

        if (page < totalPages)
            links["next"] = new() { Href = BuildHref(basePath, page + 1, pageSize) };

        if (totalPages > 0)
            links["last"] = new() { Href = BuildHref(basePath, totalPages, pageSize) };

        return links;
    }

    private static string BuildHref(string basePath, int page, int pageSize)
    {
        var separator = basePath.Contains('?') ? '&' : '?';
        return $"{basePath}{separator}page={page}&pageSize={pageSize}";
    }
}
