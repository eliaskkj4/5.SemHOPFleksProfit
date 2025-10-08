namespace FleksProfit.API.Models;

public class ProfitCalculation
{
    public int Id { get; set; }
    public DateTime CalculationDate { get; set; }
    public double Capacity { get; set; }
    public double Revenue { get; set; }
    public double ElectricityCost { get; set; }
    public double BatteryLoss { get; set; }
    public double StartupCost { get; set; }
    public double TotalProfit { get; set; }
    public string? Area { get; set; }
}
