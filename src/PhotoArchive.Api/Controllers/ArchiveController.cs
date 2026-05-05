using Microsoft.AspNetCore.Mvc;
using PhotoArchive.Data.Services;
using PhotoArchive.Core.Models;

namespace PhotoArchive.Api.Controllers;

[ApiController]
public class ArchiveController : ControllerBase
{
    private readonly PhotoService _photoService;

    public ArchiveController(PhotoService photoService)
    {
        _photoService = photoService;
    }

    [HttpGet("/years")]
    public async Task<IActionResult> GetYears()
    {
        var years = await _photoService.GetYearsAsync();
        return Ok(new { years });
    }

    [HttpGet("/years/{year:int}")]
    public async Task<IActionResult> GetYear(int year)
    {
        var response = await _photoService.GetYearAsync(year);

        if (response is null)
            return NotFound();

        return Ok(response);
    }

    [HttpGet("/years/{year:int}/photos")]
    public async Task<IActionResult> GetPhotosForYear(int year, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        var response = await _photoService.GetPhotosByYearAsync(year, page, pageSize);
        return Ok(response);
    }

    [HttpGet("/years/{year:int}/months")]
    public async Task<IActionResult> GetMonths(int year)
    {
        var months = await _photoService.GetMonthsAsync(year);
        return Ok(new { year, months });
    }

    [HttpGet("/years/{year:int}/months/{month:int}")]
    public async Task<IActionResult> GetMonth(int year, int month)
    {
        if (month < 1 || month > 12)
            return BadRequest("Month must be between 1 and 12.");

        var response = await _photoService.GetMonthAsync(year, month);

        if (response is null)
            return NotFound();

        return Ok(response);
    }

    [HttpGet("/years/{year:int}/months/{month:int}/photos")]
    public async Task<IActionResult> GetPhotosForMonth(int year, int month, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        if (month < 1 || month > 12)
            return BadRequest("Month must be between 1 and 12.");

        var response = await _photoService.GetPhotosByMonthAsync(year, month, page, pageSize);
        return Ok(response);
    }

    [HttpGet("/years/{year:int}/months/{month:int}/days")]
    public async Task<IActionResult> GetDays(int year, int month)
    {
        if (month < 1 || month > 12)
            return BadRequest("Month must be between 1 and 12.");

        var days = await _photoService.GetDaysAsync(year, month);
        return Ok(new
        {
            year,
            month,
            photoCount = days.Sum(d => d.PhotoCount),
            links = new Dictionary<string, ApiLink>
            {
                ["self"] = new() { Href = $"/years/{year}/months/{month}/days" },
                ["photos"] = new() { Href = $"/photos?year={year}&month={month}" }
            },
            days
        });
    }

    [HttpGet("/years/{year:int}/months/{month:int}/days/{day:int}")]
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

    [HttpGet("/years/{year:int}/months/{month:int}/days/{day:int}/photos")]
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