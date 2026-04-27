using System.ServiceModel;

namespace CurrencyExchange.Service;

[ServiceContract]
public interface IExchangeService
{
    [OperationContract]
    string SayHello(string name);

    [OperationContract]
    decimal GetExchangeRate(string currencyCode);
}