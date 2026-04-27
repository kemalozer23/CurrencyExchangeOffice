using System.ServiceModel;
using CurrencyExchange.Service.Models;

namespace CurrencyExchange.Service;

[ServiceContract]
public interface IExchangeService
{
    // Lab 1
    [OperationContract]
    string SayHello(string name);

    // Labs 2-4
    [OperationContract]
    decimal GetExchangeRate(string currencyCode);

    // Lab 6 - User management
    [OperationContract]
    OperationResult Register(string username, string password);

    [OperationContract]
    OperationResult Login(string username, string password);

    // Lab 6 - Balance
    [OperationContract]
    OperationResult TopUp(string username, decimal amount);

    [OperationContract]
    List<BalanceResult> GetBalances(string username);

    // Lab 6 - Exchange
    [OperationContract]
    OperationResult BuyCurrency(string username, string currencyCode, decimal amount);

    [OperationContract]
    OperationResult SellCurrency(string username, string currencyCode, decimal amount);

    // Lab 6 - History
    [OperationContract]
    List<TransactionResult> GetTransactionHistory(string username);
}