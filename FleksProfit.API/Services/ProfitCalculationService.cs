using FleksProfit.API.DTOs;
using FleksProfit.API.Models;
using FleksProfit.API.Data;
using Microsoft.EntityFrameworkCore;

namespace FleksProfit.API.Services;

public class ProfitCalculationService : IProfitCalculationService
{
    private readonly IEnerginetService _energinetService;
    private readonly IElectricityPriceService _electricityPriceService;
    private readonly FleksProfitDbContext _context;
    private readonly ILogger<ProfitCalculationService> _logger;

    public ProfitCalculationService(
        IEnerginetService energinetService,
        IElectricityPriceService electricityPriceService,
        FleksProfitDbContext context,
        ILogger<ProfitCalculationService> logger)
    {
        _energinetService = energinetService;
        _electricityPriceService = electricityPriceService;
        _context = context;
        _logger = logger;
    }

    public async Task<ProfitCalculationResponse> CalculateProfitAsync(ProfitCalculationRequest request)
    {
        try
        {
            // Fetch system performance data
            var systemPerformance = await _energinetService.GetSystemPerformanceDataAsync(
                request.StartDate, request.EndDate, request.Area);

            // Fetch electricity prices
            var electricityPrices = await _electricityPriceService.GetElectricityPricesAsync(
                request.StartDate, request.EndDate, request.Area);

            // Calculate daily profits
            var dailyDetails = new List<DailyProfitDetail>();
            double totalRevenue = 0;
            double totalCost = 0;

            // Group by date
            var dates = systemPerformance.Select(s => s.Timestamp.Date).Distinct().OrderBy(d => d);

            foreach (var date in dates)
            {
                var dayPerformance = systemPerformance.Where(s => s.Timestamp.Date == date).ToList();
                var dayPrices = electricityPrices.Where(p => p.Timestamp.Date == date).ToList();

                if (!dayPerformance.Any() || !dayPrices.Any())
                    continue;

                // Calculate revenue from regulation
                // Revenue = Capacity * (Up-regulation + Down-regulation) / 2
                var avgUpRegulation = dayPerformance.Average(p => p.UpRegulation);
                var avgDownRegulation = dayPerformance.Average(p => p.DownRegulation);
                var dailyRevenue = request.Capacity * ((avgUpRegulation + avgDownRegulation) / 2) * 24;

                // Calculate electricity cost
                var avgSpotPrice = dayPrices.Average(p => p.SpotPrice);
                var dailyCost = request.Capacity * avgSpotPrice * 24;

                // Account for battery losses
                var batteryLossCost = dailyCost * (request.BatteryLossPercentage / 100);
                
                var dailyProfit = dailyRevenue - dailyCost - batteryLossCost;

                dailyDetails.Add(new DailyProfitDetail
                {
                    Date = date,
                    Revenue = dailyRevenue,
                    Cost = dailyCost + batteryLossCost,
                    Profit = dailyProfit
                });

                totalRevenue += dailyRevenue;
                totalCost += dailyCost + batteryLossCost;
            }

            // Add startup cost to total cost
            var totalStartupCost = request.StartupCost;
            var netProfit = totalRevenue - totalCost - totalStartupCost;

            // Save calculation to database
            var calculation = new ProfitCalculation
            {
                CalculationDate = DateTime.UtcNow,
                Capacity = request.Capacity,
                Revenue = totalRevenue,
                ElectricityCost = totalCost - (totalCost * request.BatteryLossPercentage / 100),
                BatteryLoss = totalCost * request.BatteryLossPercentage / 100,
                StartupCost = totalStartupCost,
                TotalProfit = netProfit,
                Area = request.Area
            };

            _context.ProfitCalculations.Add(calculation);
            await _context.SaveChangesAsync();

            return new ProfitCalculationResponse
            {
                Capacity = request.Capacity,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Area = request.Area,
                TotalRevenue = totalRevenue,
                ElectricityCost = totalCost - (totalCost * request.BatteryLossPercentage / 100),
                BatteryLoss = totalCost * request.BatteryLossPercentage / 100,
                StartupCost = totalStartupCost,
                NetProfit = netProfit,
                DailyDetails = dailyDetails
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating profit");
            throw;
        }
    }
}
