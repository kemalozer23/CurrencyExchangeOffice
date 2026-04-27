using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using CurrencyExchange.Data;
using CurrencyExchange.Data.Entities;
using CurrencyExchange.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Service;

public class ExchangeService : IExchangeService
{
    private static readonly HttpClient HttpClient = new();
    private readonly AppDbContext _db;

    public ExchangeService(AppDbContext db)
    {
        _db = db;
    }

    // ── Helpers ────────────────────────────────────────────────

    private static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }

    private User? GetUser(string username)
    {
        return _db.Users
            .Include(u => u.Balances)
            .Include(u => u.Transactions)
            .FirstOrDefault(u => u.Username == username);
    }

    // ── Lab 1 ──────────────────────────────────────────────────

    public string SayHello(string name)
    {
        return $"Hello, {name}! Welcome to Currency Exchange Office.";
    }

    // ── Labs 2-4 ───────────────────────────────────────────────

    public decimal GetExchangeRate(string currencyCode)
    {
        try
        {
            var url = $"http://api.nbp.pl/api/exchangerates/rates/A/{currencyCode}/?format=json";
            var response = HttpClient.GetStringAsync(url).Result;
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var nbpResponse = JsonSerializer.Deserialize<NbpResponse>(response, options);
            return nbpResponse?.Rates[0].Mid ?? 0;
        }
        catch (Exception ex)
        {
            throw new Exception($"Could not fetch rate for {currencyCode}: {ex.Message}");
        }
    }

    // ── User Management ────────────────────────────────────────

    public OperationResult Register(string username, string password)
    {
        if (_db.Users.Any(u => u.Username == username))
            return new OperationResult { Success = false, Message = "Username already exists." };

        var user = new User
        {
            Username = username,
            PasswordHash = HashPassword(password)
        };

        // Every user starts with a PLN balance of 0
        user.Balances.Add(new Balance { Currency = "PLN", Amount = 0 });

        _db.Users.Add(user);
        _db.SaveChanges();

        return new OperationResult { Success = true, Message = "Registration successful." };
    }

    public OperationResult Login(string username, string password)
    {
        var user = _db.Users.FirstOrDefault(u => u.Username == username);
        if (user == null || user.PasswordHash != HashPassword(password))
            return new OperationResult { Success = false, Message = "Invalid username or password." };

        return new OperationResult { Success = true, Message = "Login successful." };
    }

    // ── Balance ────────────────────────────────────────────────

    public OperationResult TopUp(string username, decimal amount)
    {
        if (amount <= 0)
            return new OperationResult { Success = false, Message = "Amount must be greater than 0." };

        var user = GetUser(username);
        if (user == null)
            return new OperationResult { Success = false, Message = "User not found." };

        var pln = user.Balances.FirstOrDefault(b => b.Currency == "PLN");
        if (pln == null)
        {
            user.Balances.Add(new Balance { Currency = "PLN", Amount = amount });
        }
        else
        {
            pln.Amount += amount;
        }

        _db.Transactions.Add(new Transaction
        {
            UserId = user.Id,
            Type = "TOPUP",
            Currency = "PLN",
            Amount = amount,
            Rate = 1
        });

        _db.SaveChanges();
        return new OperationResult { Success = true, Message = $"Topped up {amount} PLN successfully." };
    }

    public List<BalanceResult> GetBalances(string username)
    {
        var user = GetUser(username);
        if (user == null) return new List<BalanceResult>();

        return user.Balances
            .Select(b => new BalanceResult { Currency = b.Currency, Amount = b.Amount })
            .ToList();
    }

    // ── Exchange ───────────────────────────────────────────────

    public OperationResult BuyCurrency(string username, string currencyCode, decimal amount)
    {
        var user = GetUser(username);
        if (user == null)
            return new OperationResult { Success = false, Message = "User not found." };

        var rate = GetExchangeRate(currencyCode);
        var cost = amount * rate;

        var pln = user.Balances.FirstOrDefault(b => b.Currency == "PLN");
        if (pln == null || pln.Amount < cost)
            return new OperationResult { Success = false, Message = $"Insufficient PLN balance. Required: {cost:F2} PLN." };

        pln.Amount -= cost;

        var foreignBalance = user.Balances.FirstOrDefault(b => b.Currency == currencyCode);
        if (foreignBalance == null)
        {
            user.Balances.Add(new Balance { Currency = currencyCode, Amount = amount, UserId = user.Id });
        }
        else
        {
            foreignBalance.Amount += amount;
        }

        _db.Transactions.Add(new Transaction
        {
            UserId = user.Id,
            Type = "BUY",
            Currency = currencyCode,
            Amount = amount,
            Rate = rate
        });

        _db.SaveChanges();
        return new OperationResult { Success = true, Message = $"Bought {amount} {currencyCode} at {rate} PLN each." };
    }

    public OperationResult SellCurrency(string username, string currencyCode, decimal amount)
    {
        var user = GetUser(username);
        if (user == null)
            return new OperationResult { Success = false, Message = "User not found." };

        var foreignBalance = user.Balances.FirstOrDefault(b => b.Currency == currencyCode);
        if (foreignBalance == null || foreignBalance.Amount < amount)
            return new OperationResult { Success = false, Message = $"Insufficient {currencyCode} balance." };

        var rate = GetExchangeRate(currencyCode);
        var earned = amount * rate;

        foreignBalance.Amount -= amount;

        var pln = user.Balances.FirstOrDefault(b => b.Currency == "PLN");
        if (pln == null)
        {
            user.Balances.Add(new Balance { Currency = "PLN", Amount = earned, UserId = user.Id });
        }
        else
        {
            pln.Amount += earned;
        }

        _db.Transactions.Add(new Transaction
        {
            UserId = user.Id,
            Type = "SELL",
            Currency = currencyCode,
            Amount = amount,
            Rate = rate
        });

        _db.SaveChanges();
        return new OperationResult { Success = true, Message = $"Sold {amount} {currencyCode} at {rate} PLN each." };
    }

    // ── History ────────────────────────────────────────────────

    public List<TransactionResult> GetTransactionHistory(string username)
    {
        var user = _db.Users.FirstOrDefault(u => u.Username == username);
        if (user == null) return new List<TransactionResult>();

        return _db.Transactions
            .Where(t => t.UserId == user.Id)
            .OrderByDescending(t => t.Date)
            .Select(t => new TransactionResult
            {
                Type = t.Type,
                Currency = t.Currency,
                Amount = t.Amount,
                Rate = t.Rate,
                Date = t.Date
            })
            .ToList();
    }
    
    // ── Historical Rates ───────────────────────────────────────

    public HistoricalRateResult GetHistoricalRate(string currencyCode, string date)
    {
        try
        {
            var url = $"http://api.nbp.pl/api/exchangerates/rates/A/{currencyCode}/{date}/?format=json";
            var response = HttpClient.GetStringAsync(url).Result;
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var nbpResponse = JsonSerializer.Deserialize<NbpResponse>(response, options);

            return new HistoricalRateResult
            {
                Currency = currencyCode.ToUpper(),
                Date = date,
                Rate = nbpResponse?.Rates[0].Mid ?? 0
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"Could not fetch historical rate for {currencyCode} on {date}: {ex.Message}");
        }
    }

    public List<HistoricalRateResult> GetRatesForDateRange(string currencyCode, string startDate, string endDate)
    {
        try
        {
            var url = $"http://api.nbp.pl/api/exchangerates/rates/A/{currencyCode}/{startDate}/{endDate}/?format=json";
            var response = HttpClient.GetStringAsync(url).Result;
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var nbpResponse = JsonSerializer.Deserialize<NbpResponse>(response, options);

            return nbpResponse?.Rates.Select(r => new HistoricalRateResult
            {
                Currency = currencyCode.ToUpper(),
                Date = r.EffectiveDate,
                Rate = r.Mid
            }).ToList() ?? new List<HistoricalRateResult>();
        }
        catch (Exception ex)
        {
            throw new Exception($"Could not fetch rates for {currencyCode} between {startDate} and {endDate}: {ex.Message}");
        }
    }
}