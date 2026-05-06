using Microsoft.AspNetCore.Mvc;
using PhotoArchive.Data.Services;
using PhotoArchive.Core.Models;

namespace PhotoArchive.Api.Controllers;

/// <summary>
/// Provides endpoints for navigating the photo archive by year, month, and day.
/// </summary>
[ApiController]
[Produces("application/json")]
[Tags("Archive")]
public class ArchiveController : ControllerBase
{
    private readonly PhotoService _photoService;

    /// <summary>
    /// Initializes a new instance of <see cref="ArchiveController"/>.
    /// </summary>
    /// <param name="photoService">The photo data service.</param>
    public ArchiveController(PhotoService photoService)
    {
        _photoService = photoService;
    }

    /// <summary>
    /// Returns a summary of all years that contain photos in the archive.
    /// </summary>
    /// <returns>An object with a <c>years</c> array of <see cref="YearSummaryDto"/> items.</returns>
    [HttpGet("/years")]
    [ProducesResponseType(typeof(YearsResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetYears()
    {
        var years = await _photoService.GetYearsAsync();
        return Ok(new YearsResponse { Years = years });
    }

    /// <summary>
    /// Returns the detail for a specific year, including its months and navigation links.
    /// </summary>
    /// <param name="year">The four-digit year.</param>
    /// <returns>A <see cref="YearDetailResponse"/>, or 404 if no photos exist for that year.</returns>
    [HttpGet("/years/{year:int}")]
    [ProducesResponseType(typeof(YearDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetYear(int year)
    {
        var response = await _photoService.GetYearAsync(year);

        if (response is null)
            return NotFound();

        return Ok(response);
    }

    /// <summary>
    /// Returns a paged list of photos taken in the specified year.
    /// </summary>
    /// <param name="year">The four-digit year.</param>
    /// <param name="page">The 1-based page number. Defaults to 1.</param>
    /// <param name="pageSize">The number of results per page. Defaults to 50.</param>
    /// <returns>A <see cref="PagedResponse{T}"/> of <see cref="PhotoDto"/> items.</returns>
    [HttpGet("/years/{year:int}/photos")]
    [ProducesResponseType(typeof(PagedResponse<PhotoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPhotosForYear(int year, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        var response = await _photoService.GetPhotosByYearAsync(year, page, pageSize);
        return Ok(response);
    }

    /// <summary>
    /// Returns the months within a year that contain photos.
    /// </summary>
    /// <param name="year">The four-digit year.</param>
    /// <returns>An object with <c>year</c> and a <c>months</c> array of <see cref="MonthSummaryDto"/> items.</returns>
    [HttpGet("/years/{year:int}/months")]
    [ProducesResponseType(typeof(MonthsResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMonths(int year)
    {
        var months = await _photoService.GetMonthsAsync(year);
        return Ok(new MonthsResponse { Year = year, Months = months });
    }

    /// <summary>
    /// Returns the detail for a specific year/month, including its days and navigation links.
    /// </summary>
    /// <param name="year">The four-digit year.</param>
    /// <param name="month">The month number (1–12).</param>
    /// <returns>A <see cref="MonthDetailResponse"/>, or 404 if no photos exist for that month.</returns>
    [HttpGet("/years/{year:int}/months/{month:int}")]
    [ProducesResponseType(typeof(MonthDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMonth(int year, int month)
    {
        if (month < 1 || month > 12)
            return BadRequest("Month must be between 1 and 12.");

        var response = await _photoService.GetMonthAsync(year, month);

        if (response is null)
            return NotFound();

        return Ok(response);
    }

    /// <summary>
    /// Returns a paged list of photos taken in the specified year and month.
    /// </summary>
    /// <param name="year">The four-digit year.</param>
    /// <param name="month">The month number (1–12).</param>
    /// <param name="page">The 1-based page number. Defaults to 1.</param>
    /// <param name="pageSize">The number of results per page. Defaults to 50.</param>
    /// <returns>A <see cref="PagedResponse{T}"/> of <see cref="PhotoDto"/> items.</returns>
    [HttpGet("/years/{year:int}/months/{month:int}/photos")]
    [ProducesResponseType(typeof(PagedResponse<PhotoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPhotosForMonth(int year, int month, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        if (month < 1 || month > 12)
            return BadRequest("Month must be between 1 and 12.");

        var response = await _photoService.GetPhotosByMonthAsync(year, month, page, pageSize);
        return Ok(response);
    }

    /// <summary>
    /// Returns the days within a given year/month that contain photos, along with a photo count and navigation links.
    /// </summary>
    /// <param name="year">The four-digit year.</param>
    /// <param name="month">The month number (1–12).</param>
    /// <returns>A <see cref="DaysResponse"/> with <c>year</c>, <c>month</c>, <c>photoCount</c>, <c>links</c>, and a <c>days</c> array of <see cref="DaySummaryDto"/> items.</returns>
    [HttpGet("/years/{year:int}/months/{month:int}/days")]
    [ProducesResponseType(typeof(DaysResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetDays(int year, int month)
    {
        if (month < 1 || month > 12)
            return BadRequest("Month must be between 1 and 12.");

        var days = await _photoService.GetDaysAsync(year, month);
        return Ok(new DaysResponse
        {
            Year = year,
            Month = month,
            PhotoCount = days.Sum(d => d.PhotoCount),
            Links = new Dictionary<string, ApiLink>
            {
                ["self"] = new() { Href = $"/years/{year}/months/{month}/days" },
                ["photos"] = new() { Href = $"/years/{year}/months/{month}/photos" },
                ["query"] = new() { Href = $"/photos?year={year}&month={month}" }
            },
            Days = days
        });
    }

    /// <summary>
    /// Returns the detail for a specific year/month/day, including photo count and navigation links.
    /// </summary>
    /// <param name="year">The four-digit year.</param>
    /// <param name="month">The month number (1–12).</param>
    /// <param name="day">The day of the month (1–31).</param>
    /// <returns>A <see cref="DayDetailResponse"/>, or 404 if no photos exist on that date.</returns>
    [HttpGet("/years/{year:int}/months/{month:int}/days/{day:int}")]
    [ProducesResponseType(typeof(DayDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDay(int year, int month, int day)
    {
        if (month < 1 || month > 12)
            return BadRequest("Month must be between 1 and 12.");

        if (day < 1 || day > 31)
            return BadRequest("Day must be between 1 and 31.");

        var response = await _photoService.GetDayAsync(year, month, day);

        if (response is null)
            return NotFound();

        return Ok(response);
    }

    /// <summary>
    /// Returns a paged list of photos taken on the specified year/month/day.
    /// </summary>
    /// <param name="year">The four-digit year.</param>
    /// <param name="month">The month number (1–12).</param>
    /// <param name="day">The day of the month (1–31).</param>
    /// <param name="page">The 1-based page number. Defaults to 1.</param>
    /// <param name="pageSize">The number of results per page. Defaults to 50.</param>
    /// <returns>A <see cref="PagedResponse{T}"/> of <see cref="PhotoDto"/> items.</returns>
    [HttpGet("/years/{year:int}/months/{month:int}/days/{day:int}/photos")]
    [ProducesResponseType(typeof(PagedResponse<PhotoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPhotosForDay(int year, int month, int day, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        if (month < 1 || month > 12)
            return BadRequest("Month must be between 1 and 12.");

        if (day < 1 || day > 31)
            return BadRequest("Day must be between 1 and 31.");

        var response = await _photoService.GetPhotosByDayAsync(year, month, day, page, pageSize);
        return Ok(response);
    }
}
