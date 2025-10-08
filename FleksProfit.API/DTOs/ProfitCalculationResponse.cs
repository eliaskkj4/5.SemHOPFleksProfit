namespace FleksProfit.API.DTOs;

public class ProfitCalculationResponse
{
    public double Capacity { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Area { get; set; } = string.Empty;
    public double TotalRevenue { get; set; }
    public double ElectricityCost { get; set; }
    public double BatteryLoss { get; set; }
    public double StartupCost { get; set; }
    public double NetProfit { get; set; }
    public List<DailyProfitDetail> DailyDetails { get; set; } = new();
}

public class DailyProfitDetail
{
    public DateTime Date { get; set; }
    public double Revenue { get; set; }
    public double Cost { get; set; }
    public double Profit { get; set; }
}
