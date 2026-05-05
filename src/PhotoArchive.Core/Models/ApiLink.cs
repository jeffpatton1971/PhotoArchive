namespace PhotoArchive.Core.Models;

/// <summary>
/// Represents a hypermedia link included in API responses.
/// </summary>
public class ApiLink
{
    /// <summary>Gets or sets the URL of the link.</summary>
    public string Href { get; set; } = default!;
}