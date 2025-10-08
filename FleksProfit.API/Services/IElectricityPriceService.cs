using FleksProfit.API.Models;

namespace FleksProfit.API.Services;

public interface IElectricityPriceService
{
    Task<List<ElectricityPrice>> GetElectricityPricesAsync(DateTime startDate, DateTime endDate, string area);
}
