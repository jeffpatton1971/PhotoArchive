namespace PhotoArchive.Core.Models;

public class PagedResponse<T>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }

    public List<T> Items { get; set; } = [];

    public Dictionary<string, ApiLink> Links { get; set; } = [];
}