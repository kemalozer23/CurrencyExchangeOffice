using System.ServiceModel;
using CurrencyExchange.Service;

namespace CurrencyExchange.UI.Services;

public class ExchangeClient
{
    private readonly IExchangeService _client;

    public ExchangeClient()
    {
        var binding = new BasicHttpBinding
        {
            Security = { Mode = BasicHttpSecurityMode.None }
        };
        var endpoint = new EndpointAddress("http://localhost:5050/ExchangeService");
        var factory = new ChannelFactory<IExchangeService>(binding, endpoint);
        _client = factory.CreateChannel();
    }

    public IExchangeService Client => _client;
}