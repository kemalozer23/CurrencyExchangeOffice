using System.ServiceModel;
using CurrencyExchange.Service;

var binding = new BasicHttpBinding
{
    Security = { Mode = BasicHttpSecurityMode.None }
};

var endpoint = new EndpointAddress("http://localhost:5050/ExchangeService");
var factory = new ChannelFactory<IExchangeService>(binding, endpoint);
var client = factory.CreateChannel();

// Test hello
Console.WriteLine(client.SayHello("Kemal"));

// Test exchange rates
Console.WriteLine($"\n-- Exchange Rates --");
Console.WriteLine($"1 USD = {client.GetExchangeRate("USD")} PLN");
Console.WriteLine($"1 EUR = {client.GetExchangeRate("EUR")} PLN");

// Test registration
Console.WriteLine($"\n-- Register --");
var reg = client.Register("kemal", "password123");
Console.WriteLine(reg.Message);

// Test login
Console.WriteLine($"\n-- Login --");
var login = client.Login("kemal", "password123");
Console.WriteLine(login.Message);

// Test top up
Console.WriteLine($"\n-- Top Up --");
var topUp = client.TopUp("kemal", 1000);
Console.WriteLine(topUp.Message);

// Test buy
Console.WriteLine($"\n-- Buy USD --");
var buy = client.BuyCurrency("kemal", "USD", 10);
Console.WriteLine(buy.Message);

// Test balances
Console.WriteLine($"\n-- Balances --");
var balances = client.GetBalances("kemal");
foreach (var b in balances)
    Console.WriteLine($"{b.Currency}: {b.Amount:F2}");

// Test sell
Console.WriteLine($"\n-- Sell USD --");
var sell = client.SellCurrency("kemal", "USD", 5);
Console.WriteLine(sell.Message);

// Test history
Console.WriteLine($"\n-- Transaction History --");
var history = client.GetTransactionHistory("kemal");
foreach (var t in history)
    Console.WriteLine($"{t.Date:yyyy-MM-dd HH:mm} | {t.Type} | {t.Amount} {t.Currency} @ {t.Rate}");

// Test historical rate
Console.WriteLine($"\n-- Historical Rate --");
var historical = client.GetHistoricalRate("USD", "2024-01-15");
Console.WriteLine($"USD on {historical.Date}: {historical.Rate} PLN");

// Test date range
Console.WriteLine($"\n-- USD Rates Jan 2024 --");
var range = client.GetRatesForDateRange("USD", "2024-01-01", "2024-01-10");
foreach (var r in range)
    Console.WriteLine($"{r.Date}: {r.Rate} PLN");

((IClientChannel)client).Close();
factory.Close();