namespace CurrencyExchange.Data.Entities;

public class Transaction
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Type { get; set; } = string.Empty; // BUY or SELL or TOPUP
    public string Currency { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal Rate { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    
    public User User { get; set; } = null!;
}