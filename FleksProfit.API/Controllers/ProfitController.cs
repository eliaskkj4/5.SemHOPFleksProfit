using Microsoft.AspNetCore.Mvc;
using FleksProfit.API.DTOs;
using FleksProfit.API.Services;

namespace FleksProfit.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfitController : ControllerBase
{
    private readonly IProfitCalculationService _profitCalculationService;
    private readonly ILogger<ProfitController> _logger;

    public ProfitController(
        IProfitCalculationService profitCalculationService,
        ILogger<ProfitController> logger)
    {
        _profitCalculationService = profitCalculationService;
        _logger = logger;
    }

    /// <summary>
    /// Calculate profit from capacity offering
    /// </summary>
    /// <param name="request">Profit calculation request parameters</param>
    /// <returns>Profit calculation results</returns>
    [HttpPost("calculate")]
    [ProducesResponseType(typeof(ProfitCalculationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProfitCalculationResponse>> CalculateProfit(
        [FromBody] ProfitCalculationRequest request)
    {
        try
        {
            if (request.Capacity <= 0)
            {
                return BadRequest("Capacity must be greater than 0");
            }

            if (request.StartDate >= request.EndDate)
            {
                return BadRequest("Start date must be before end date");
            }

            var result = await _profitCalculationService.CalculateProfitAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating profit");
            return StatusCode(500, "An error occurred while calculating profit");
        }
    }
}
