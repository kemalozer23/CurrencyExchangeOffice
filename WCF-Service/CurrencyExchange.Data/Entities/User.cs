namespace CurrencyExchange.Data.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<Balance> Balances { get; set; } = new List<Balance>();
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}