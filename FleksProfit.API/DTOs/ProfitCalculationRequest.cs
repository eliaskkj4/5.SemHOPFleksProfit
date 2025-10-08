namespace FleksProfit.API.DTOs;

public class ProfitCalculationRequest
{
    public double Capacity { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Area { get; set; } = "DK1";
    public double BatteryLossPercentage { get; set; } = 15.0;
    public double StartupCost { get; set; } = 0.0;
}
