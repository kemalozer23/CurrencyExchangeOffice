namespace CurrencyExchange.Data.Entities;

public class Balance
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal Amount { get; set; } = 0;
    
    public User User { get; set; } = null!;
}