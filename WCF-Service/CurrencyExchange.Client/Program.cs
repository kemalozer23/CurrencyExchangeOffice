using System.ServiceModel;
using CurrencyExchange.Service;

var binding = new BasicHttpBinding
{
    Security = { Mode = BasicHttpSecurityMode.None }
};

var endpoint = new EndpointAddress("http://localhost:5050/ExchangeService");
var factory = new ChannelFactory<IExchangeService>(binding, endpoint);
var client = factory.CreateChannel();

Console.WriteLine(client.SayHello("Kemal"));

string[] currencies = ["USD", "EUR", "GBP"];
foreach (var currency in currencies)
{
    var rate = client.GetExchangeRate(currency);
    Console.WriteLine($"1 {currency} = {rate} PLN");
}

((IClientChannel)client).Close();
factory.Close();