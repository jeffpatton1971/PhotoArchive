using System.Net;
using System.Text.Json;
using PhotoArchive.Core.Models;

namespace PhotoArchive.Tests.Integration;

public class PostsEndpointTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client;

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public PostsEndpointTests(ApiFactory factory)
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

    // ── 15. Post detail ───────────────────────────────────────────────────────

    [Fact]
    public async Task GetPostSummary_KnownPostId_Returns200()
    {
        var response = await _client.GetAsync($"/posts/{SeedData.KnownPostId}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetPostSummary_KnownPostId_HasCorrectPostIdAndPhotoCount()
    {
        var response = await _client.GetAsync($"/posts/{SeedData.KnownPostId}");
        var result = JsonSerializer.Deserialize<PostSummaryResponse>(
            await response.Content.ReadAsStringAsync(), JsonOptions);
        Assert.NotNull(result);
        Assert.Equal(SeedData.KnownPostId, result.PostId);
        Assert.Equal(7, result.PhotoCount);
    }

    [Fact]
    public async Task GetPostSummary_KnownPostId_LinksContainSelfAndPhotos()
    {
        var response = await _client.GetAsync($"/posts/{SeedData.KnownPostId}");
        var result = JsonSerializer.Deserialize<PostSummaryResponse>(
            await response.Content.ReadAsStringAsync(), JsonOptions);
        Assert.NotNull(result);
        Assert.True(result.Links.ContainsKey("self"));
        Assert.True(result.Links.ContainsKey("photos"));
        Assert.Contains(SeedData.KnownPostId, result.Links["self"].Href);
    }

    [Fact]
    public async Task GetPostSummary_UnknownPostId_Returns404()
    {
        var response = await _client.GetAsync("/posts/no-such-post-id-xyz");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // ── 16. Post photos ───────────────────────────────────────────────────────

    [Fact]
    public async Task GetPostPhotos_KnownPostId_ReturnsPagedResponse()
    {
        var result = await GetPagedAsync(
            $"/posts/{SeedData.KnownPostId}/photos?page=1&pageSize=5");

        Assert.Equal(7, result.TotalCount);
        Assert.Equal(1, result.Page);
        Assert.Equal(5, result.PageSize);
    }

    [Fact]
    public async Task GetPostPhotos_KnownPostId_AllItemsMatchPostId()
    {
        var result = await GetPagedAsync(
            $"/posts/{SeedData.KnownPostId}/photos?page=1&pageSize=5");

        Assert.All(result.Items, p => Assert.Equal(SeedData.KnownPostId, p.PostId));
    }

    [Fact]
    public async Task GetPostPhotos_PaginationLinks_UsePostRoute()
    {
        var result = await GetPagedAsync(
            $"/posts/{SeedData.KnownPostId}/photos?page=1&pageSize=5");

        var expectedPrefix = $"/posts/{SeedData.KnownPostId}/photos";
        Assert.StartsWith(expectedPrefix, result.Links["self"].Href);
        Assert.StartsWith(expectedPrefix, result.Links["first"].Href);
        Assert.StartsWith(expectedPrefix, result.Links["last"].Href);
    }

    [Fact]
    public async Task GetPostPhotos_Page2_HasPreviousLinkAndCorrectRoute()
    {
        var result = await GetPagedAsync(
            $"/posts/{SeedData.KnownPostId}/photos?page=2&pageSize=5");

        Assert.True(result.Links.ContainsKey("previous"));
        Assert.StartsWith($"/posts/{SeedData.KnownPostId}/photos", result.Links["previous"].Href);
    }
}
