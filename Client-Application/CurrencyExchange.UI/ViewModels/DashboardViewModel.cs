using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CurrencyExchange.Service.Models;
using CurrencyExchange.UI.Services;

namespace CurrencyExchange.UI.ViewModels;

public class DashboardViewModel : ViewModelBase
{
    private readonly MainViewModel _main;
    private readonly ExchangeClient _client;
    private readonly string _username;

    private string _message = string.Empty;
    private string _topUpAmount = string.Empty;
    private string _currency = "USD";
    private string _tradeAmount = string.Empty;
    private decimal _currentRate;

    public ObservableCollection<BalanceResult> Balances { get; } = new();

    public DashboardViewModel(MainViewModel main, ExchangeClient client, string username)
    {
        _main = main;
        _client = client;
        _username = username;

        TopUpCommand = new RelayCommand(TopUp);
        BuyCommand = new RelayCommand(Buy);
        SellCommand = new RelayCommand(Sell);
        ViewHistoryCommand = new RelayCommand(() => _main.NavigateToHistory(username));
        LogoutCommand = new RelayCommand(() => _main.NavigateToLogin());
        CheckRateCommand = new RelayCommand(CheckRate);

        LoadBalances();
    }

    public string Username => _username;
    public string Message { get => _message; set => SetField(ref _message, value); }
    public string TopUpAmount { get => _topUpAmount; set => SetField(ref _topUpAmount, value); }
    public string Currency { get => _currency; set => SetField(ref _currency, value); }
    public string TradeAmount { get => _tradeAmount; set => SetField(ref _tradeAmount, value); }
    public decimal CurrentRate { get => _currentRate; set => SetField(ref _currentRate, value); }

    public ICommand TopUpCommand { get; }
    public ICommand BuyCommand { get; }
    public ICommand SellCommand { get; }
    public ICommand ViewHistoryCommand { get; }
    public ICommand LogoutCommand { get; }
    public ICommand CheckRateCommand { get; }

    private void LoadBalances()
    {
        try
        {
            Balances.Clear();
            var balances = _client.Client.GetBalances(_username);
            foreach (var b in balances)
                Balances.Add(b);
        }
        catch (Exception ex) { Message = $"Could not load balances: {ex.Message}"; }
    }
    private void CheckRate()
    {
        if (string.IsNullOrWhiteSpace(Currency)) return;
        try
        {
            CurrentRate = _client.Client.GetExchangeRate(Currency.ToUpper());
            Message = $"1 {Currency.ToUpper()} = {CurrentRate} PLN";
        }
        catch (Exception ex) { Message = $"Service error: {ex.Message}"; }
    }

    private void TopUp()
    {
        if (!decimal.TryParse(TopUpAmount, out var amount))
        {
            Message = "Invalid amount.";
            return;
        }
        try
        {
            var result = _client.Client.TopUp(_username, amount);
            Message = result.Message;
            LoadBalances();
        }
        catch (Exception ex) { Message = $"Service error: {ex.Message}"; }
    }

    private void Buy()
    {
        if (!decimal.TryParse(TradeAmount, out var amount))
        {
            Message = "Invalid amount.";
            return;
        }
        try
        {
            var result = _client.Client.BuyCurrency(_username, Currency.ToUpper(), amount);
            Message = result.Message;
            LoadBalances();
        }
        catch (Exception ex) { Message = $"Service error: {ex.Message}"; }
    }
    private void Sell()
    {
        if (!decimal.TryParse(TradeAmount, out var amount))
        {
            Message = "Invalid amount.";
            return;
        }
        try
        {
            var result = _client.Client.SellCurrency(_username, Currency.ToUpper(), amount);
            Message = result.Message;
            LoadBalances();
        }
        catch (Exception ex) { Message = $"Service error: {ex.Message}"; }
    }
}