using FleksProfit.API.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FleksProfit.API.Services;

public class ElectricityPriceService : IElectricityPriceService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ElectricityPriceService> _logger;
    private const string BaseUrl = "https://api.energidataservice.dk/dataset/";

    public ElectricityPriceService(HttpClient httpClient, ILogger<ElectricityPriceService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<ElectricityPrice>> GetElectricityPricesAsync(DateTime startDate, DateTime endDate, string area)
    {
        try
        {
            // Using Energinet's spot price dataset as Strømlignings API uses similar data
            var dataset = "Elspotprices";
            var start = startDate.ToString("yyyy-MM-dd'T'HH:mm");
            var end = endDate.ToString("yyyy-MM-dd'T'HH:mm");
            
            var url = $"{BaseUrl}{dataset}?start={start}&end={end}&filter={{\"PriceArea\":\"{area}\"}}";
            
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var data = JsonSerializer.Deserialize<SpotPriceApiResponse>(content, options);
            
            if (data?.Records == null)
            {
                _logger.LogWarning("No price data received from API");
                return new List<ElectricityPrice>();
            }
            
            return data.Records.Select(r => new ElectricityPrice
            {
                Timestamp = r.HourUTC ?? r.HourDK ?? DateTime.UtcNow,
                Area = area,
                SpotPrice = r.SpotPriceDKK ?? r.SpotPriceEUR ?? 0,
                Currency = r.SpotPriceDKK.HasValue ? "DKK" : "EUR"
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching electricity prices");
            return new List<ElectricityPrice>();
        }
    }
}

// Response models for Spot Price API
public class SpotPriceApiResponse
{
    public List<SpotPriceRecord> Records { get; set; } = new();
}

public class SpotPriceRecord
{
    [JsonPropertyName("HourUTC")]
    public DateTime? HourUTC { get; set; }
    
    [JsonPropertyName("HourDK")]
    public DateTime? HourDK { get; set; }
    
    [JsonPropertyName("PriceArea")]
    public string? PriceArea { get; set; }
    
    [JsonPropertyName("SpotPriceDKK")]
    public double? SpotPriceDKK { get; set; }
    
    [JsonPropertyName("SpotPriceEUR")]
    public double? SpotPriceEUR { get; set; }
}
