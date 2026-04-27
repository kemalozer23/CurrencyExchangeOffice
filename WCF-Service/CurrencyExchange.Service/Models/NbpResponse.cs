namespace CurrencyExchange.Service.Models;

public class NbpResponse
{
    public string Currency { get; set; }
    public string Code { get; set; }
    public List<Rate> Rates { get; set; }
}

public class Rate
{
    public string No { get; set; }
    public string EffectiveDate { get; set; }
    public decimal Mid { get; set; }
}