namespace FleksProfit.API.Models;

public class SystemPerformance
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string? Area { get; set; }
    public double UpRegulation { get; set; }
    public double DownRegulation { get; set; }
    public double Frequency { get; set; }
    public double CapacityUtilization { get; set; }
}
