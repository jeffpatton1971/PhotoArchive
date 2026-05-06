using PhotoArchive.Core.Entities;
using PhotoArchive.Data;

namespace PhotoArchive.Tests.Integration;

/// <summary>
/// Deterministic seed constants and data builder for API integration tests.
/// The dataset is designed to be small enough to load quickly while providing
/// enough records to exercise pagination (pageSize=5 with 7+ matching photos).
/// </summary>
internal static class SeedData
{
    // ── Known constants used by test assertions ───────────────────────────────
    public const int KnownYear = 2022;
    public const int KnownMonth = 5;
    public const int KnownDay = 5;
    public const string KnownSource = "instagram";
    public const string KnownGallery = "instagram-2022-05-05-184310-day-1-in-the-books";
    public const string KnownPostId = "2022-05-05-184310-day-1-in-the-books";
    public const string KnownSlug = "instagram-2022-05-05-184310-day-1-in-the-books-01";

    // ── Seed counts ────────────────────────────────────────────────────────────
    // 7 instagram photos on 2022-05-05 (same gallery + postId, sortIndex 1–7)
    // 3 instagram photos on 2022-05-10
    // 2 facebook photos on 2022-06-15
    // 1 photo from 2021-03-20

    public static void Seed(PhotoDbContext db)
    {
        var photos = new List<Photo>();

        // 7 instagram photos on 2022-05-05 with known gallery + postId
        for (int i = 1; i <= 7; i++)
        {
            photos.Add(new Photo
            {
                Id = Guid.NewGuid(),
                Slug = i == 1
                    ? KnownSlug
                    : $"{KnownGallery}-{i:D2}",
                Title = $"Day 1 in the Books – photo {i}",
                Gallery = KnownGallery,
                PostId = KnownPostId,
                PostUrl = $"https://example.com/posts/{KnownPostId}",
                SortIndex = i,
                Source = KnownSource,
                TakenAt = new DateTimeOffset(KnownYear, KnownMonth, KnownDay, 18, 43, i * 5, TimeSpan.Zero),
                Year = KnownYear,
                Month = KnownMonth,
                Day = KnownDay,
                OriginalUrl = $"https://blob.example.com/photos/{KnownGallery}-{i:D2}.jpg"
            });
        }

        // 3 instagram photos on 2022-05-10
        for (int i = 1; i <= 3; i++)
        {
            photos.Add(new Photo
            {
                Id = Guid.NewGuid(),
                Slug = $"instagram-2022-05-10-photo-{i:D2}",
                Source = KnownSource,
                TakenAt = new DateTimeOffset(KnownYear, 5, 10, 12, 0, i * 10, TimeSpan.Zero),
                Year = KnownYear,
                Month = 5,
                Day = 10,
                OriginalUrl = $"https://blob.example.com/photos/instagram-2022-05-10-{i:D2}.jpg"
            });
        }

        // 2 facebook photos on 2022-06-15
        for (int i = 1; i <= 2; i++)
        {
            photos.Add(new Photo
            {
                Id = Guid.NewGuid(),
                Slug = $"facebook-2022-06-15-photo-{i:D2}",
                Source = "facebook",
                TakenAt = new DateTimeOffset(KnownYear, 6, 15, 9, 0, 0, TimeSpan.Zero),
                Year = KnownYear,
                Month = 6,
                Day = 15,
                OriginalUrl = $"https://blob.example.com/photos/facebook-2022-06-15-{i:D2}.jpg"
            });
        }

        // 1 photo from 2021-03-20
        photos.Add(new Photo
        {
            Id = Guid.NewGuid(),
            Slug = "legacy-2021-03-20-photo-01",
            Source = "legacy",
            TakenAt = new DateTimeOffset(2021, 3, 20, 10, 0, 0, TimeSpan.Zero),
            Year = 2021,
            Month = 3,
            Day = 20,
            OriginalUrl = "https://blob.example.com/photos/legacy-2021-03-20-01.jpg"
        });

        db.Photos.AddRange(photos);
        db.SaveChanges();
    }
}
