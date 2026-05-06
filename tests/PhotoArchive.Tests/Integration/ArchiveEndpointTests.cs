using System.Net;
using System.Text.Json;
using PhotoArchive.Core.Models;

namespace PhotoArchive.Tests.Integration;

public class ArchiveEndpointTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client;

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public ArchiveEndpointTests(ApiFactory factory)
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

    // ── 7. Archive root ───────────────────────────────────────────────────────

    [Fact]
    public async Task GetYears_ReturnsOk_WithYearsArray()
    {
        var response = await _client.GetAsync("/years");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var doc = JsonSerializer.Deserialize<JsonElement>(
            await response.Content.ReadAsStringAsync(), JsonOptions);

        Assert.True(doc.TryGetProperty("years", out var yearsEl), "Response must have 'years' property.");
        Assert.Equal(JsonValueKind.Array, yearsEl.ValueKind);
        Assert.True(yearsEl.GetArrayLength() > 0);
    }

    [Fact]
    public async Task GetYears_EachEntry_HasExpectedLinks()
    {
        var response = await _client.GetAsync("/years");
        var doc = JsonSerializer.Deserialize<JsonElement>(
            await response.Content.ReadAsStringAsync(), JsonOptions);

        var years = doc.GetProperty("years").EnumerateArray().ToList();
        foreach (var yr in years)
        {
            Assert.True(yr.TryGetProperty("links", out var links), "Year entry must have 'links'.");
            Assert.True(links.TryGetProperty("months", out _), "Year links must have 'months'.");
            Assert.True(links.TryGetProperty("photos", out _), "Year links must have 'photos'.");
            Assert.True(links.TryGetProperty("query", out _), "Year links must have 'query'.");
        }
    }

    [Fact]
    public async Task GetYears_ContainsKnownYear()
    {
        var response = await _client.GetAsync("/years");
        var doc = JsonSerializer.Deserialize<JsonElement>(
            await response.Content.ReadAsStringAsync(), JsonOptions);

        var years = doc.GetProperty("years").EnumerateArray();
        Assert.Contains(years, yr =>
            yr.TryGetProperty("year", out var y) && y.GetInt32() == SeedData.KnownYear);
    }

    // ── 8. Year detail ────────────────────────────────────────────────────────

    [Fact]
    public async Task GetYear_KnownYear_Returns200WithCorrectData()
    {
        var response = await _client.GetAsync($"/years/{SeedData.KnownYear}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = JsonSerializer.Deserialize<YearDetailResponse>(
            await response.Content.ReadAsStringAsync(), JsonOptions);
        Assert.NotNull(result);
        Assert.Equal(SeedData.KnownYear, result.Year);
        Assert.True(result.PhotoCount > 0);
        Assert.NotEmpty(result.Links);
    }

    [Fact]
    public async Task GetYear_UnknownYear_Returns404()
    {
        var response = await _client.GetAsync("/years/1900");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // ── 9. Year photos ────────────────────────────────────────────────────────

    [Fact]
    public async Task GetPhotosForYear_ReturnsPagedResponse_WithCorrectYear()
    {
        var result = await GetPagedAsync($"/years/{SeedData.KnownYear}/photos?page=1&pageSize=5");

        Assert.Equal(1, result.Page);
        Assert.Equal(5, result.PageSize);
        Assert.All(result.Items, p => Assert.Equal(SeedData.KnownYear, p.Year));
    }

    [Fact]
    public async Task GetPhotosForYear_PaginationLinks_UseArchiveRoute()
    {
        var result = await GetPagedAsync($"/years/{SeedData.KnownYear}/photos?page=1&pageSize=5");

        var self = result.Links["self"].Href;
        Assert.StartsWith($"/years/{SeedData.KnownYear}/photos", self);
        Assert.DoesNotContain("/photos?year=", self);

        Assert.StartsWith($"/years/{SeedData.KnownYear}/photos", result.Links["first"].Href);
        Assert.StartsWith($"/years/{SeedData.KnownYear}/photos", result.Links["last"].Href);
    }

    // ── 10. Month detail ──────────────────────────────────────────────────────

    [Fact]
    public async Task GetMonth_KnownYearMonth_Returns200WithCorrectData()
    {
        var response = await _client.GetAsync($"/years/{SeedData.KnownYear}/months/{SeedData.KnownMonth}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = JsonSerializer.Deserialize<MonthDetailResponse>(
            await response.Content.ReadAsStringAsync(), JsonOptions);
        Assert.NotNull(result);
        Assert.Equal(SeedData.KnownYear, result.Year);
        Assert.Equal(SeedData.KnownMonth, result.Month);
        Assert.True(result.PhotoCount > 0);
        Assert.NotEmpty(result.Links);
    }

    [Fact]
    public async Task GetMonth_UnknownYearMonth_Returns404()
    {
        var response = await _client.GetAsync("/years/1900/months/1");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // ── 11. Month photos ──────────────────────────────────────────────────────

    [Fact]
    public async Task GetPhotosForMonth_ReturnsPagedResponse_WithMatchingYearMonth()
    {
        var result = await GetPagedAsync(
            $"/years/{SeedData.KnownYear}/months/{SeedData.KnownMonth}/photos?page=1&pageSize=5");

        Assert.All(result.Items, p =>
        {
            Assert.Equal(SeedData.KnownYear, p.Year);
            Assert.Equal(SeedData.KnownMonth, p.Month);
        });
    }

    [Fact]
    public async Task GetPhotosForMonth_PaginationLinks_UseArchiveRoute()
    {
        var result = await GetPagedAsync(
            $"/years/{SeedData.KnownYear}/months/{SeedData.KnownMonth}/photos?page=1&pageSize=5");

        var expectedPrefix = $"/years/{SeedData.KnownYear}/months/{SeedData.KnownMonth}/photos";
        Assert.StartsWith(expectedPrefix, result.Links["self"].Href);
        Assert.StartsWith(expectedPrefix, result.Links["first"].Href);
        Assert.StartsWith(expectedPrefix, result.Links["last"].Href);
    }

    // ── 12. Day detail ────────────────────────────────────────────────────────

    [Fact]
    public async Task GetDay_KnownDate_Returns200WithCorrectData()
    {
        var response = await _client.GetAsync(
            $"/years/{SeedData.KnownYear}/months/{SeedData.KnownMonth}/days/{SeedData.KnownDay}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = JsonSerializer.Deserialize<DayDetailResponse>(
            await response.Content.ReadAsStringAsync(), JsonOptions);
        Assert.NotNull(result);
        Assert.Equal(SeedData.KnownYear, result.Year);
        Assert.Equal(SeedData.KnownMonth, result.Month);
        Assert.Equal(SeedData.KnownDay, result.Day);
        Assert.Equal(7, result.PhotoCount);
        Assert.NotEmpty(result.Links);
    }

    [Fact]
    public async Task GetDay_UnknownDate_Returns404()
    {
        var response = await _client.GetAsync("/years/1900/months/1/days/1");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // ── 13. Day photos ────────────────────────────────────────────────────────

    [Fact]
    public async Task GetPhotosForDay_ReturnsPagedResponse_WithMatchingDate()
    {
        var result = await GetPagedAsync(
            $"/years/{SeedData.KnownYear}/months/{SeedData.KnownMonth}/days/{SeedData.KnownDay}/photos?page=1&pageSize=5");

        Assert.Equal(7, result.TotalCount);
        Assert.All(result.Items, p =>
        {
            Assert.Equal(SeedData.KnownYear, p.Year);
            Assert.Equal(SeedData.KnownMonth, p.Month);
            Assert.Equal(SeedData.KnownDay, p.Day);
        });
    }

    [Fact]
    public async Task GetPhotosForDay_PaginationLinks_UseArchiveRoute()
    {
        var result = await GetPagedAsync(
            $"/years/{SeedData.KnownYear}/months/{SeedData.KnownMonth}/days/{SeedData.KnownDay}/photos?page=1&pageSize=5");

        var expectedPrefix =
            $"/years/{SeedData.KnownYear}/months/{SeedData.KnownMonth}/days/{SeedData.KnownDay}/photos";
        Assert.StartsWith(expectedPrefix, result.Links["self"].Href);
        Assert.StartsWith(expectedPrefix, result.Links["first"].Href);
        Assert.StartsWith(expectedPrefix, result.Links["last"].Href);
        Assert.DoesNotContain("/photos?year=", result.Links["self"].Href);
    }
}
