using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CurrencyExchange.Service.Models;
using CurrencyExchange.UI.Services;

namespace CurrencyExchange.UI.ViewModels;

public class HistoryViewModel : ViewModelBase
{
    private readonly MainViewModel _main;
    private readonly ExchangeClient _client;
    private readonly string _username;

    public ObservableCollection<TransactionResult> Transactions { get; } = new();

    public HistoryViewModel(MainViewModel main, ExchangeClient client, string username)
    {
        _main = main;
        _client = client;
        _username = username;
        BackCommand = new RelayCommand(() => _main.NavigateToDashboard(username));
        LoadHistory();
    }

    public ICommand BackCommand { get; }

    private void LoadHistory()
    {
        try
        {
            Transactions.Clear();
            var history = _client.Client.GetTransactionHistory(_username);
            foreach (var t in history)
                Transactions.Add(t);
        }
        catch (Exception ex)
        {
            // silently fail, history just won't load
            Console.WriteLine($"History error: {ex.Message}");
        }
    }
}