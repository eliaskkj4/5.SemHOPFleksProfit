using FleksProfit.API.DTOs;

namespace FleksProfit.API.Services;

public interface IProfitCalculationService
{
    Task<ProfitCalculationResponse> CalculateProfitAsync(ProfitCalculationRequest request);
}
