using PhotoArchive.Core.Models;

namespace PhotoArchive.Tests;

/// <summary>
/// Unit tests for <see cref="ResourceLinkBuilder"/> verifying that every method
/// returns the exact path string expected by the API routes.
/// </summary>
public class ResourceLinkBuilderTests
{
    // ── Photos ────────────────────────────────────────────────────────────────

    [Fact]
    public void Photos_ReturnsPhotosPath()
        => Assert.Equal("/photos", ResourceLinkBuilder.Photos());

    [Fact]
    public void PhotoSelf_ReturnsSlugPath()
        => Assert.Equal("/photos/my-slug", ResourceLinkBuilder.PhotoSelf("my-slug"));

    [Fact]
    public void PhotoSelf_EscapesSlug()
        => Assert.Equal("/photos/a%20b", ResourceLinkBuilder.PhotoSelf("a b"));

    [Fact]
    public void PhotosQuery_NoFilters_ReturnsPhotosPath()
        => Assert.Equal("/photos", ResourceLinkBuilder.PhotosQuery());

    [Fact]
    public void PhotosQuery_YearOnly_ReturnsYearParam()
        => Assert.Equal("/photos?year=2022", ResourceLinkBuilder.PhotosQuery(year: 2022));

    [Fact]
    public void PhotosQuery_YearMonthDay_ReturnsAllParams()
        => Assert.Equal("/photos?year=2022&month=5&day=5",
            ResourceLinkBuilder.PhotosQuery(year: 2022, month: 5, day: 5));

    [Fact]
    public void PhotosQuery_SourceFilter_ContainsSource()
    {
        var result = ResourceLinkBuilder.PhotosQuery(source: "instagram");
        Assert.Contains("source=instagram", result);
        Assert.StartsWith("/photos?", result);
    }

    [Fact]
    public void PhotosQuery_GalleryFilter_EscapesGallery()
    {
        var result = ResourceLinkBuilder.PhotosQuery(gallery: "my gallery");
        Assert.Contains("gallery=my%20gallery", result);
    }

    [Fact]
    public void PhotosQuery_PostIdFilter_EscapesPostId()
    {
        var result = ResourceLinkBuilder.PhotosQuery(postId: "post/id");
        Assert.Contains("postId=post%2Fid", result);
    }

    // ── Years ─────────────────────────────────────────────────────────────────

    [Fact]
    public void Years_ReturnsYearsPath()
        => Assert.Equal("/years", ResourceLinkBuilder.Years());

    [Fact]
    public void Year_ReturnsYearPath()
        => Assert.Equal("/years/2022", ResourceLinkBuilder.Year(2022));

    [Fact]
    public void YearPhotos_ReturnsYearPhotosPath()
        => Assert.Equal("/years/2022/photos", ResourceLinkBuilder.YearPhotos(2022));

    [Fact]
    public void YearMonths_ReturnsYearMonthsPath()
        => Assert.Equal("/years/2022/months", ResourceLinkBuilder.YearMonths(2022));

    // ── Months ────────────────────────────────────────────────────────────────

    [Fact]
    public void Month_ReturnsMonthPath()
        => Assert.Equal("/years/2022/months/5", ResourceLinkBuilder.Month(2022, 5));

    [Fact]
    public void MonthPhotos_ReturnsMonthPhotosPath()
        => Assert.Equal("/years/2022/months/5/photos", ResourceLinkBuilder.MonthPhotos(2022, 5));

    [Fact]
    public void MonthDays_ReturnsMonthDaysPath()
        => Assert.Equal("/years/2022/months/5/days", ResourceLinkBuilder.MonthDays(2022, 5));

    // ── Days ──────────────────────────────────────────────────────────────────

    [Fact]
    public void Day_ReturnsDayPath()
        => Assert.Equal("/years/2022/months/5/days/5", ResourceLinkBuilder.Day(2022, 5, 5));

    [Fact]
    public void DayPhotos_ReturnsDayPhotosPath()
        => Assert.Equal("/years/2022/months/5/days/5/photos", ResourceLinkBuilder.DayPhotos(2022, 5, 5));

    // ── Posts ─────────────────────────────────────────────────────────────────

    [Fact]
    public void Post_ReturnsPostPath()
        => Assert.Equal("/posts/my-post", ResourceLinkBuilder.Post("my-post"));

    [Fact]
    public void Post_EscapesPostId()
        => Assert.Equal("/posts/a%2Fb", ResourceLinkBuilder.Post("a/b"));

    [Fact]
    public void PostPhotos_ReturnsPostPhotosPath()
        => Assert.Equal("/posts/my-post/photos", ResourceLinkBuilder.PostPhotos("my-post"));

    // ── Galleries ─────────────────────────────────────────────────────────────

    [Fact]
    public void Gallery_ReturnsGalleryPath()
        => Assert.Equal("/galleries/my-gallery", ResourceLinkBuilder.Gallery("my-gallery"));

    [Fact]
    public void GalleryPhotos_ReturnsGalleryPhotosPath()
        => Assert.Equal("/galleries/my-gallery/photos", ResourceLinkBuilder.GalleryPhotos("my-gallery"));

    [Fact]
    public void GalleryPhotos_EscapesGalleryName()
    {
        var result = ResourceLinkBuilder.GalleryPhotos("my gallery");
        Assert.Equal("/galleries/my%20gallery/photos", result);
    }

    // ── On This Day ───────────────────────────────────────────────────────────

    [Fact]
    public void OnThisDay_ReturnsOnThisDayPath()
        => Assert.Equal("/on-this-day?month=5&day=5", ResourceLinkBuilder.OnThisDay(5, 5));

    [Fact]
    public void OnThisDay_Month12Day31_CorrectPath()
        => Assert.Equal("/on-this-day?month=12&day=31", ResourceLinkBuilder.OnThisDay(12, 31));
}
