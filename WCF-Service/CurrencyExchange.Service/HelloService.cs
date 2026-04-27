namespace CurrencyExchange.Service;

public class HelloService : IHelloService
{
    public string SayHello(string name)
    {
        return $"Hello, {name}! Welcome to Currency Exchange Office.";
    }
}