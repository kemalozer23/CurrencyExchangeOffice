using System.ServiceModel;
using CurrencyExchange.Service;

var binding = new BasicHttpBinding
{
    Security =
    {
        Mode = BasicHttpSecurityMode.None
    }
};

var endpoint = new EndpointAddress("http://localhost:5050/HelloService");

var factory = new ChannelFactory<IHelloService>(binding, endpoint);
var client = factory.CreateChannel();

var result = client.SayHello("Kemal");
Console.WriteLine(result);

((IClientChannel)client).Close();
factory.Close();