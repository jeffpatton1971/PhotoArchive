using System.Net;
using System.Text.Json;
using PhotoArchive.Core.Models;

namespace PhotoArchive.Tests.Integration;

public class OnThisDayEndpointTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client;

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public OnThisDayEndpointTests(ApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    // ── 14. On this day ───────────────────────────────────────────────────────

    [Fact]
    public async Task GetOnThisDay_KnownMonthDay_Returns200()
    {
        var response = await _client.GetAsync(
            $"/on-this-day?month={SeedData.KnownMonth}&day={SeedData.KnownDay}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetOnThisDay_KnownMonthDay_ResponseContainsMonthAndDay()
    {
        var response = await _client.GetAsync(
            $"/on-this-day?month={SeedData.KnownMonth}&day={SeedData.KnownDay}");

        var result = JsonSerializer.Deserialize<OnThisDayResponse>(
            await response.Content.ReadAsStringAsync(), JsonOptions);
        Assert.NotNull(result);
        Assert.Equal(SeedData.KnownMonth, result.Month);
        Assert.Equal(SeedData.KnownDay, result.Day);
    }

    [Fact]
    public async Task GetOnThisDay_KnownMonthDay_ReturnsGroupedYears()
    {
        var response = await _client.GetAsync(
            $"/on-this-day?month={SeedData.KnownMonth}&day={SeedData.KnownDay}");

        var result = JsonSerializer.Deserialize<OnThisDayResponse>(
            await response.Content.ReadAsStringAsync(), JsonOptions);
        Assert.NotNull(result);
        Assert.NotEmpty(result.Years);
    }

    [Fact]
    public async Task GetOnThisDay_KnownMonthDay_AllPhotosMatchMonthAndDay()
    {
        var response = await _client.GetAsync(
            $"/on-this-day?month={SeedData.KnownMonth}&day={SeedData.KnownDay}");

        var result = JsonSerializer.Deserialize<OnThisDayResponse>(
            await response.Content.ReadAsStringAsync(), JsonOptions);
        Assert.NotNull(result);

        foreach (var group in result.Years)
        {
            Assert.All(group.Photos, p =>
            {
                Assert.Equal(SeedData.KnownMonth, p.Month);
                Assert.Equal(SeedData.KnownDay, p.Day);
            });
        }
    }

    [Fact]
    public async Task GetOnThisDay_KnownMonthDay_ContainsKnownYearGroup()
    {
        var response = await _client.GetAsync(
            $"/on-this-day?month={SeedData.KnownMonth}&day={SeedData.KnownDay}");

        var result = JsonSerializer.Deserialize<OnThisDayResponse>(
            await response.Content.ReadAsStringAsync(), JsonOptions);
        Assert.NotNull(result);
        Assert.Contains(result.Years, g => g.Year == SeedData.KnownYear);
    }

    [Fact]
    public async Task GetOnThisDay_InvalidMonth_Returns400()
    {
        var response = await _client.GetAsync("/on-this-day?month=13&day=1");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetOnThisDay_InvalidDay_Returns400()
    {
        var response = await _client.GetAsync("/on-this-day?month=5&day=32");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
