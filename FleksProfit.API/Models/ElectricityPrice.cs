namespace FleksProfit.API.Models;

public class ElectricityPrice
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string? Area { get; set; }
    public double SpotPrice { get; set; }
    public string? Currency { get; set; }
}
