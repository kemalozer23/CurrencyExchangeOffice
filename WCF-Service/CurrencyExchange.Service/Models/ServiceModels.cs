using System.Runtime.Serialization;

namespace CurrencyExchange.Service.Models;

[DataContract]
public class OperationResult
{
    [DataMember] public bool Success { get; set; }
    [DataMember] public string Message { get; set; } = string.Empty;
}

[DataContract]
public class BalanceResult
{
    [DataMember] public string Currency { get; set; } = string.Empty;
    [DataMember] public decimal Amount { get; set; }
}

[DataContract]
public class TransactionResult
{
    [DataMember] public string Type { get; set; } = string.Empty;
    [DataMember] public string Currency { get; set; } = string.Empty;
    [DataMember] public decimal Amount { get; set; }
    [DataMember] public decimal Rate { get; set; }
    [DataMember] public DateTime Date { get; set; }
}