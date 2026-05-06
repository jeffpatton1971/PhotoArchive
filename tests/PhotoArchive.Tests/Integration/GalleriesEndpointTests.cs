using System.Net;
using System.Text.Json;
using PhotoArchive.Core.Models;

namespace PhotoArchive.Tests.Integration;

public class GalleriesEndpointTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client;

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public GalleriesEndpointTests(ApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    private async Task<PagedResponse<PhotoDto>> GetPagedAsync(string url)
    {
        var response = await _client.GetAsync(url);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = JsonSerializer.Deserialize<PagedResponse<PhotoDto>>(
            await response.Content.ReadAsStringAsync(), JsonOptions);
        Assert.NotNull(result);
        return result;
    }

    // ── 17. Gallery photos ────────────────────────────────────────────────────

    [Fact]
    public async Task GetGalleryPhotos_KnownGallery_Returns200()
    {
        var encodedGallery = Uri.EscapeDataString(SeedData.KnownGallery);
        var response = await _client.GetAsync($"/galleries/{encodedGallery}/photos?page=1&pageSize=5");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetGalleryPhotos_KnownGallery_ReturnsPagedResponse()
    {
        var encodedGallery = Uri.EscapeDataString(SeedData.KnownGallery);
        var result = await GetPagedAsync($"/galleries/{encodedGallery}/photos?page=1&pageSize=5");

        Assert.Equal(7, result.TotalCount);
        Assert.Equal(1, result.Page);
        Assert.Equal(5, result.PageSize);
    }

    [Fact]
    public async Task GetGalleryPhotos_KnownGallery_AllItemsMatchGallery()
    {
        var encodedGallery = Uri.EscapeDataString(SeedData.KnownGallery);
        var result = await GetPagedAsync($"/galleries/{encodedGallery}/photos?page=1&pageSize=5");

        Assert.All(result.Items, p => Assert.Equal(SeedData.KnownGallery, p.Gallery));
    }

    [Fact]
    public async Task GetGalleryPhotos_PaginationLinks_UseGalleryRoute()
    {
        var encodedGallery = Uri.EscapeDataString(SeedData.KnownGallery);
        var result = await GetPagedAsync($"/galleries/{encodedGallery}/photos?page=1&pageSize=5");

        var expectedPrefix = $"/galleries/{encodedGallery}/photos";
        Assert.StartsWith(expectedPrefix, result.Links["self"].Href);
        Assert.StartsWith(expectedPrefix, result.Links["first"].Href);
        Assert.StartsWith(expectedPrefix, result.Links["last"].Href);
    }

    [Fact]
    public async Task GetGalleryPhotos_Page2_HasPreviousLinkAndCorrectRoute()
    {
        var encodedGallery = Uri.EscapeDataString(SeedData.KnownGallery);
        var result = await GetPagedAsync($"/galleries/{encodedGallery}/photos?page=2&pageSize=5");

        Assert.True(result.Links.ContainsKey("previous"));
        Assert.StartsWith($"/galleries/{encodedGallery}/photos", result.Links["previous"].Href);
    }

    [Fact]
    public async Task GetGalleryPhotos_LastPage_NoNextLink()
    {
        var encodedGallery = Uri.EscapeDataString(SeedData.KnownGallery);
        var first = await GetPagedAsync($"/galleries/{encodedGallery}/photos?page=1&pageSize=5");
        var last = await GetPagedAsync(
            $"/galleries/{encodedGallery}/photos?page={first.TotalPages}&pageSize=5");

        Assert.False(last.Links.ContainsKey("next"),
            "Last gallery page must not include a 'next' link.");
    }

    [Fact]
    public async Task GetGalleryPhotos_PageSizePreservedInLinks()
    {
        const int pageSize = 5;
        var encodedGallery = Uri.EscapeDataString(SeedData.KnownGallery);
        var result = await GetPagedAsync($"/galleries/{encodedGallery}/photos?page=1&pageSize={pageSize}");

        foreach (var link in result.Links.Values)
        {
            Assert.Contains($"pageSize={pageSize}", link.Href);
        }
    }
}
