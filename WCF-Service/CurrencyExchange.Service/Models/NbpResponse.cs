namespace CurrencyExchange.Service.Models;

public class NbpResponse
{
    public string Currency { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public List<Rate> Rates { get; set; } = [];
}

public class Rate
{
    public string No { get; set; } = string.Empty;
    public string EffectiveDate { get; set; } = string.Empty;
    public decimal Mid { get; set; }
}