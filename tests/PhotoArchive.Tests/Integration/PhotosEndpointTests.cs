using System.Net;
using System.Text.Json;
using PhotoArchive.Core.Models;

namespace PhotoArchive.Tests.Integration;

public class PhotosEndpointTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client;

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public PhotosEndpointTests(ApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    // ── Helper ────────────────────────────────────────────────────────────────

    private async Task<PagedResponse<PhotoDto>> GetPagedPhotosAsync(string url)
    {
        var response = await _client.GetAsync(url);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<PagedResponse<PhotoDto>>(json, JsonOptions);
        Assert.NotNull(result);
        return result;
    }

    // ── 1. Photos collection ──────────────────────────────────────────────────

    [Fact]
    public async Task GetPhotos_ReturnsOk_WithPagedShape()
    {
        var result = await GetPagedPhotosAsync("/photos?page=1&pageSize=5");

        Assert.True(result.TotalCount > 0);
        Assert.True(result.TotalPages > 0);
        Assert.Equal(1, result.Page);
        Assert.Equal(5, result.PageSize);
        Assert.NotNull(result.Items);
        Assert.NotNull(result.Links);
    }

    [Fact]
    public async Task GetPhotos_PagedResponse_ContainsSelfFirstLastLinks()
    {
        var result = await GetPagedPhotosAsync("/photos?page=1&pageSize=5");

        Assert.True(result.Links.ContainsKey("self"));
        Assert.True(result.Links.ContainsKey("first"));
        Assert.True(result.Links.ContainsKey("last"));
    }

    // ── 2. Date filter ────────────────────────────────────────────────────────

    [Fact]
    public async Task GetPhotos_DateFilter_ReturnsOnlyMatchingDayRecords()
    {
        var result = await GetPagedPhotosAsync(
            $"/photos?year={SeedData.KnownYear}&month={SeedData.KnownMonth}&day={SeedData.KnownDay}&page=1&pageSize=10");

        Assert.All(result.Items, p =>
        {
            Assert.Equal(SeedData.KnownYear, p.Year);
            Assert.Equal(SeedData.KnownMonth, p.Month);
            Assert.Equal(SeedData.KnownDay, p.Day);
        });
    }

    [Fact]
    public async Task GetPhotos_DateFilter_PaginationLinksPreserveDateParams()
    {
        var result = await GetPagedPhotosAsync(
            $"/photos?year={SeedData.KnownYear}&month={SeedData.KnownMonth}&day={SeedData.KnownDay}&page=1&pageSize=5");

        var self = result.Links["self"].Href;
        Assert.Contains($"year={SeedData.KnownYear}", self);
        Assert.Contains($"month={SeedData.KnownMonth}", self);
        Assert.Contains($"day={SeedData.KnownDay}", self);
    }

    // ── 3. Source filter ──────────────────────────────────────────────────────

    [Fact]
    public async Task GetPhotos_SourceFilter_ReturnsOnlyMatchingSource()
    {
        var result = await GetPagedPhotosAsync(
            $"/photos?source={SeedData.KnownSource}&page=1&pageSize=5");

        Assert.All(result.Items, p => Assert.Equal(SeedData.KnownSource, p.Source));
    }

    [Fact]
    public async Task GetPhotos_SourceFilter_PaginationLinksPreserveSourceParam()
    {
        // This specifically protects the previously found bug where links dropped filters.
        var result = await GetPagedPhotosAsync(
            $"/photos?source={SeedData.KnownSource}&page=1&pageSize=5");

        var self = result.Links["self"].Href;
        Assert.Contains($"source={SeedData.KnownSource}", self);

        if (result.Links.TryGetValue("next", out var next))
            Assert.Contains($"source={SeedData.KnownSource}", next.Href);

        Assert.Contains($"source={SeedData.KnownSource}", result.Links["first"].Href);
        Assert.Contains($"source={SeedData.KnownSource}", result.Links["last"].Href);
    }

    // ── 4. Gallery filter ─────────────────────────────────────────────────────

    [Fact]
    public async Task GetPhotos_GalleryFilter_ReturnsOnlyMatchingGallery()
    {
        var result = await GetPagedPhotosAsync(
            $"/photos?gallery={Uri.EscapeDataString(SeedData.KnownGallery)}&page=1&pageSize=5");

        Assert.All(result.Items, p => Assert.Equal(SeedData.KnownGallery, p.Gallery));
    }

    [Fact]
    public async Task GetPhotos_GalleryFilter_PaginationLinksPreserveGalleryParam()
    {
        var result = await GetPagedPhotosAsync(
            $"/photos?gallery={Uri.EscapeDataString(SeedData.KnownGallery)}&page=1&pageSize=5");

        var self = result.Links["self"].Href;
        Assert.Contains("gallery=", self);

        if (result.Links.TryGetValue("next", out var next))
            Assert.Contains("gallery=", next.Href);
    }

    // ── 5. PostId filter ──────────────────────────────────────────────────────

    [Fact]
    public async Task GetPhotos_PostIdFilter_ReturnsOnlyMatchingPostId()
    {
        var result = await GetPagedPhotosAsync(
            $"/photos?postId={Uri.EscapeDataString(SeedData.KnownPostId)}&page=1&pageSize=5");

        Assert.All(result.Items, p => Assert.Equal(SeedData.KnownPostId, p.PostId));
    }

    [Fact]
    public async Task GetPhotos_PostIdFilter_PaginationLinksPreservePostIdParam()
    {
        var result = await GetPagedPhotosAsync(
            $"/photos?postId={Uri.EscapeDataString(SeedData.KnownPostId)}&page=1&pageSize=5");

        var self = result.Links["self"].Href;
        Assert.Contains("postId=", self);

        if (result.Links.TryGetValue("next", out var next))
            Assert.Contains("postId=", next.Href);
    }

    // ── 6. Photo detail ───────────────────────────────────────────────────────

    [Fact]
    public async Task GetPhotoBySlug_KnownSlug_Returns200WithMatchingSlug()
    {
        var response = await _client.GetAsync($"/photos/{SeedData.KnownSlug}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<PhotoDetailResponse>(json, JsonOptions);
        Assert.NotNull(result);
        Assert.Equal(SeedData.KnownSlug, result.Photo.Slug);
    }

    [Fact]
    public async Task GetPhotoBySlug_KnownSlug_LinksContainSelf()
    {
        var response = await _client.GetAsync($"/photos/{SeedData.KnownSlug}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<PhotoDetailResponse>(json, JsonOptions);
        Assert.NotNull(result);
        Assert.True(result.Links.ContainsKey("self"));
        Assert.Contains(SeedData.KnownSlug, result.Links["self"].Href);
    }

    [Fact]
    public async Task GetPhotoBySlug_KnownSlug_LinksContainGalleryAndPost()
    {
        var response = await _client.GetAsync($"/photos/{SeedData.KnownSlug}");
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<PhotoDetailResponse>(json, JsonOptions);
        Assert.NotNull(result);
        Assert.True(result.Links.ContainsKey("galleryPhotos"));
        Assert.True(result.Links.ContainsKey("postPhotos"));
    }

    [Fact]
    public async Task GetPhotoBySlug_UnknownSlug_Returns404()
    {
        var response = await _client.GetAsync("/photos/does-not-exist-slug-xyz");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // ── Pagination edge cases ─────────────────────────────────────────────────

    [Fact]
    public async Task GetPhotos_Page1_NoPreviousLink()
    {
        var result = await GetPagedPhotosAsync($"/photos?source={SeedData.KnownSource}&page=1&pageSize=5");

        Assert.False(result.Links.ContainsKey("previous"),
            "Page 1 must not include a 'previous' link.");
    }

    [Fact]
    public async Task GetPhotos_Page2_HasPreviousLink()
    {
        // The 10 instagram photos (7 on 05-05 + 3 on 05-10) give 2 pages at pageSize=5.
        var result = await GetPagedPhotosAsync($"/photos?source={SeedData.KnownSource}&page=2&pageSize=5");

        Assert.True(result.Links.ContainsKey("previous"),
            "Page 2 must include a 'previous' link.");
    }

    [Fact]
    public async Task GetPhotos_LastPage_NoNextLink()
    {
        // Fetch last page explicitly.
        var first = await GetPagedPhotosAsync($"/photos?source={SeedData.KnownSource}&page=1&pageSize=5");
        var lastPageResult = await GetPagedPhotosAsync(
            $"/photos?source={SeedData.KnownSource}&page={first.TotalPages}&pageSize=5");

        Assert.False(lastPageResult.Links.ContainsKey("next"),
            "The last page must not include a 'next' link.");
    }

    [Fact]
    public async Task GetPhotos_TotalPagesIsCorrect()
    {
        // 7 instagram photos on 2022-05-05; pageSize=5 → 2 pages
        var result = await GetPagedPhotosAsync(
            $"/photos?year={SeedData.KnownYear}&month={SeedData.KnownMonth}&day={SeedData.KnownDay}&page=1&pageSize=5");

        Assert.Equal(7, result.TotalCount);
        Assert.Equal(2, result.TotalPages);
    }

    [Fact]
    public async Task GetPhotos_PaginationLinks_NeverGeneratePage0()
    {
        var result = await GetPagedPhotosAsync($"/photos?source={SeedData.KnownSource}&page=1&pageSize=5");

        foreach (var link in result.Links.Values)
        {
            Assert.DoesNotContain("page=0", link.Href);
        }
    }

    [Fact]
    public async Task GetPhotos_PageSizeIsPreservedInAllLinks()
    {
        const int pageSize = 5;
        var result = await GetPagedPhotosAsync(
            $"/photos?source={SeedData.KnownSource}&page=1&pageSize={pageSize}");

        foreach (var link in result.Links.Values)
        {
            Assert.Contains($"pageSize={pageSize}", link.Href);
        }
    }

    // ── Filter equivalence ─────────────────────────────────────────────────────

    [Fact]
    public async Task GetPhotos_YearFilter_AndYearArchiveRoute_ReturnEquivalentItems()
    {
        // GET /photos?year=2022 vs GET /years/2022/photos should return the same item set.
        var flat = await GetPagedPhotosAsync($"/photos?year={SeedData.KnownYear}&page=1&pageSize=100");
        var archive = await GetPagedPhotosAsync($"/years/{SeedData.KnownYear}/photos?page=1&pageSize=100");

        var flatSlugs = flat.Items.Select(p => p.Slug).OrderBy(s => s).ToList();
        var archiveSlugs = archive.Items.Select(p => p.Slug).OrderBy(s => s).ToList();

        Assert.Equal(flatSlugs, archiveSlugs);
    }
}
