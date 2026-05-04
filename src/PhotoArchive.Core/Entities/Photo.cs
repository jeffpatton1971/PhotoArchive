namespace PhotoArchive.Core.Entities
{
    public class Photo
    {
        public Guid Id { get; set; }

        public string Slug { get; set; } = default!;
        public string? Title { get; set; }

        public DateTimeOffset? TakenAt { get; set; }

        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }

        public string Source { get; set; } = default!;

        public string OriginalUrl { get; set; } = default!;
        public string? ThumbUrl { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}