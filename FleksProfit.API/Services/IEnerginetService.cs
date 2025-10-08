using FleksProfit.API.Models;

namespace FleksProfit.API.Services;

public interface IEnerginetService
{
    Task<List<SystemPerformance>> GetSystemPerformanceDataAsync(DateTime startDate, DateTime endDate, string area);
}
