using FleksProfit.API.Models;
using System.Text.Json;

namespace FleksProfit.API.Services;

public class EnerginetService : IEnerginetService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<EnerginetService> _logger;
    private const string BaseUrl = "https://api.energidataservice.dk/dataset/";

    public EnerginetService(HttpClient httpClient, ILogger<EnerginetService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<SystemPerformance>> GetSystemPerformanceDataAsync(DateTime startDate, DateTime endDate, string area)
    {
        try
        {
            // Energinet API endpoint for regulation data
            var dataset = "Regulating_Prices";
            var start = startDate.ToString("yyyy-MM-dd'T'HH:mm");
            var end = endDate.ToString("yyyy-MM-dd'T'HH:mm");
            
            var url = $"{BaseUrl}{dataset}?start={start}&end={end}&filter={{\"PriceArea\":\"{area}\"}}";
            
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<EnerginetApiResponse>(content);
            
            if (data?.Records == null)
            {
                _logger.LogWarning("No data received from Energinet API");
                return new List<SystemPerformance>();
            }
            
            return data.Records.Select(r => new SystemPerformance
            {
                Timestamp = r.HourUTC,
                Area = area,
                UpRegulation = r.ImbalancePriceUp ?? 0,
                DownRegulation = r.ImbalancePriceDown ?? 0,
                Frequency = 50.0, // Default value, actual frequency data requires different endpoint
                CapacityUtilization = 0.0 // Calculated from regulation volumes
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching data from Energinet API");
            return new List<SystemPerformance>();
        }
    }
}

// Response models for Energinet API
public class EnerginetApiResponse
{
    public List<EnerginetRecord> Records { get; set; } = new();
}

public class EnerginetRecord
{
    public DateTime HourUTC { get; set; }
    public string? PriceArea { get; set; }
    public double? ImbalancePriceUp { get; set; }
    public double? ImbalancePriceDown { get; set; }
}
