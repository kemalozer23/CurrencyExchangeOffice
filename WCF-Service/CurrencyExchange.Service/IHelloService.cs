using System.ServiceModel;

namespace CurrencyExchange.Service;

[ServiceContract]
public interface IHelloService
{
    [OperationContract]
    string SayHello(string name);
}