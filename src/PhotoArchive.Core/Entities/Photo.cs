namespace PhotoArchive.Core.Entities
{
    /// <summary>
    /// Represents a single photo record stored in the archive.
    /// </summary>
    public class Photo
    {
        /// <summary>Gets or sets the unique identifier for the photo.</summary>
        public Guid Id { get; set; }

        /// <summary>Gets or sets the URL-safe unique slug used to identify the photo.</summary>
        public string Slug { get; set; } = default!;

        /// <summary>Gets or sets the optional display title of the photo.</summary>
        public string? Title { get; set; }

        /// <summary>Gets or sets the name of the gallery this photo belongs to, if any.</summary>
        public string? Gallery { get; set; }

        /// <summary>Gets or sets the URL of the blog post associated with this photo, if any.</summary>
        public string? PostUrl { get; set; }

        /// <summary>Gets or sets the identifier of the blog post associated with this photo, if any.</summary>
        public string? PostId { get; set; }

        /// <summary>Gets or sets the sort order of this photo within its gallery or post.</summary>
        public int? SortIndex { get; set; }

        /// <summary>Gets or sets the original source filename of the photo, if known.</summary>
        public string? SourceFilename { get; set; }

        /// <summary>Gets or sets the date and time the photo was taken.</summary>
        public DateTimeOffset? TakenAt { get; set; }

        /// <summary>Gets or sets the year the photo was taken, derived from <see cref="TakenAt"/>.</summary>
        public int? Year { get; set; }

        /// <summary>Gets or sets the month the photo was taken, derived from <see cref="TakenAt"/>.</summary>
        public int? Month { get; set; }

        /// <summary>Gets or sets the day of the month the photo was taken, derived from <see cref="TakenAt"/>.</summary>
        public int? Day { get; set; }

        /// <summary>Gets or sets the import source identifier (e.g., "legacy", "facebook").</summary>
        public string Source { get; set; } = default!;

        /// <summary>Gets or sets the URL to the full-resolution image in Azure Blob Storage.</summary>
        public string OriginalUrl { get; set; } = default!;

        /// <summary>Gets or sets the URL to the thumbnail image, if available.</summary>
        public string? ThumbUrl { get; set; }

        /// <summary>Gets or sets the raw YAML front-matter serialized as JSON for auditing purposes.</summary>
        public string? SourceMetadataJson { get; set; }

        /// <summary>Gets or sets the date and time this record was created in the archive.</summary>
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}