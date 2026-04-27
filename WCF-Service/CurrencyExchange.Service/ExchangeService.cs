using System.Text.Json;
using System.Text.Json.Serialization;
using CurrencyExchange.Service.Models;

namespace CurrencyExchange.Service;

public class ExchangeService : IExchangeService
{
    private static readonly HttpClient HttpClient = new();

    public string SayHello(string name)
    {
        return $"Hello, {name}! Welcome to Currency Exchange Office.";
    }

    public decimal GetExchangeRate(string currencyCode)
    {
        try
        {
            var url = $"http://api.nbp.pl/api/exchangerates/rates/A/{currencyCode}/?format=json";
            var response = HttpClient.GetStringAsync(url).Result;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var nbpResponse = JsonSerializer.Deserialize<NbpResponse>(response, options);
            return nbpResponse?.Rates[0].Mid ?? 0;
        }
        catch (Exception ex)
        {
            throw new Exception($"Could not fetch rate for {currencyCode}: {ex.Message}");
        }
    }
}