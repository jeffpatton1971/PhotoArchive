using Microsoft.AspNetCore.Mvc;
using PhotoArchive.Data.Services;
using PhotoArchive.Core.Models;

namespace PhotoArchive.Api.Controllers;

/// <summary>
/// Provides the "On This Day" endpoint, which returns photos grouped by year for a given month and day.
/// </summary>
[ApiController]
public class OnThisDayController : ControllerBase
{
    private readonly PhotoService _photoService;

    /// <summary>
    /// Initializes a new instance of <see cref="OnThisDayController"/>.
    /// </summary>
    /// <param name="photoService">The photo data service.</param>
    public OnThisDayController(PhotoService photoService)
    {
        _photoService = photoService;
    }

    /// <summary>
    /// Returns photos grouped by year for a given month and day, defaulting to today's UTC date if not specified.
    /// </summary>
    /// <param name="month">Optional month override (1–12). Defaults to the current UTC month.</param>
    /// <param name="day">Optional day override (1–31). Defaults to the current UTC day.</param>
    /// <returns>An <see cref="OnThisDayResponse"/> grouped by year.</returns>
    [HttpGet("/on-this-day")]
    public async Task<IActionResult> GetOnThisDay([FromQuery] int? month, [FromQuery] int? day)
    {
        var targetDate = DateTime.UtcNow;

        var targetMonth = month ?? targetDate.Month;
        var targetDay = day ?? targetDate.Day;

        if (targetMonth < 1 || targetMonth > 12)
            return BadRequest("Month must be between 1 and 12.");

        if (targetDay < 1 || targetDay > 31)
            return BadRequest("Day must be between 1 and 31.");

        var response = await _photoService.GetOnThisDayGroupedAsync(targetMonth, targetDay);

        return Ok(response);
    }
}
