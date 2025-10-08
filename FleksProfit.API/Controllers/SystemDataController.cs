using Microsoft.AspNetCore.Mvc;
using FleksProfit.API.Services;
using FleksProfit.API.Models;

namespace FleksProfit.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SystemDataController : ControllerBase
{
    private readonly IEnerginetService _energinetService;
    private readonly IElectricityPriceService _electricityPriceService;
    private readonly ILogger<SystemDataController> _logger;

    public SystemDataController(
        IEnerginetService energinetService,
        IElectricityPriceService electricityPriceService,
        ILogger<SystemDataController> logger)
    {
        _energinetService = energinetService;
        _electricityPriceService = electricityPriceService;
        _logger = logger;
    }

    /// <summary>
    /// Get system performance data from Energinet
    /// </summary>
    /// <param name="startDate">Start date (ISO 8601 format)</param>
    /// <param name="endDate">End date (ISO 8601 format)</param>
    /// <param name="area">Price area (DK1 or DK2)</param>
    /// <returns>List of system performance data</returns>
    [HttpGet("performance")]
    [ProducesResponseType(typeof(List<SystemPerformance>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<SystemPerformance>>> GetSystemPerformance(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] string area = "DK1")
    {
        if (startDate >= endDate)
        {
            return BadRequest("Start date must be before end date");
        }

        var data = await _energinetService.GetSystemPerformanceDataAsync(startDate, endDate, area);
        return Ok(data);
    }

    /// <summary>
    /// Get electricity spot prices
    /// </summary>
    /// <param name="startDate">Start date (ISO 8601 format)</param>
    /// <param name="endDate">End date (ISO 8601 format)</param>
    /// <param name="area">Price area (DK1 or DK2)</param>
    /// <returns>List of electricity prices</returns>
    [HttpGet("prices")]
    [ProducesResponseType(typeof(List<ElectricityPrice>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<ElectricityPrice>>> GetElectricityPrices(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] string area = "DK1")
    {
        if (startDate >= endDate)
        {
            return BadRequest("Start date must be before end date");
        }

        var data = await _electricityPriceService.GetElectricityPricesAsync(startDate, endDate, area);
        return Ok(data);
    }
}
