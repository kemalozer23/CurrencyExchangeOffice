using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CurrencyExchange.Service.Models;
using CurrencyExchange.UI.Services;

namespace CurrencyExchange.UI.ViewModels;

public class RatesViewModel : ViewModelBase
{
    private readonly MainViewModel _main;
    private readonly ExchangeClient _client;
    private readonly string _username;

    private string _currency = "USD";
    private string _singleDate = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
    private string _startDate = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd");
    private string _endDate = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
    private string _message = string.Empty;
    private decimal _currentRate;
    private HistoricalRateResult? _singleResult;

    public ObservableCollection<HistoricalRateResult> RangeResults { get; } = new();

    public RatesViewModel(MainViewModel main, ExchangeClient client, string username)
    {
        _main = main;
        _client = client;
        _username = username;

        CheckCurrentRateCommand = new RelayCommand(CheckCurrentRate);
        CheckSingleDateCommand = new RelayCommand(CheckSingleDate);
        CheckRangeCommand = new RelayCommand(CheckRange);
        BackCommand = new RelayCommand(() => _main.NavigateToDashboard(username));
    }

    public string Currency { get => _currency; set => SetField(ref _currency, value); }
    public string SingleDate { get => _singleDate; set => SetField(ref _singleDate, value); }
    public string StartDate { get => _startDate; set => SetField(ref _startDate, value); }
    public string EndDate { get => _endDate; set => SetField(ref _endDate, value); }
    public string Message { get => _message; set => SetField(ref _message, value); }
    public decimal CurrentRate { get => _currentRate; set => SetField(ref _currentRate, value); }
    public HistoricalRateResult? SingleResult { get => _singleResult; set => SetField(ref _singleResult, value); }

    public ICommand CheckCurrentRateCommand { get; }
    public ICommand CheckSingleDateCommand { get; }
    public ICommand CheckRangeCommand { get; }
    public ICommand BackCommand { get; }

    private void CheckCurrentRate()
    {
        if (string.IsNullOrWhiteSpace(Currency)) return;
        try
        {
            CurrentRate = _client.Client.GetExchangeRate(Currency.ToUpper());
            Message = $"Current rate: 1 {Currency.ToUpper()} = {CurrentRate} PLN";
        }
        catch (Exception ex) { Message = $"Error: {ex.Message}"; }
    }

    private void CheckSingleDate()
    {
        if (string.IsNullOrWhiteSpace(Currency) || string.IsNullOrWhiteSpace(SingleDate)) return;
        try
        {
            SingleResult = _client.Client.GetHistoricalRate(Currency.ToUpper(), SingleDate);
            Message = $"Rate on {SingleDate}: 1 {Currency.ToUpper()} = {SingleResult.Rate} PLN";
        }
        catch (Exception ex) { Message = $"Error: {ex.Message}"; }
    }

    private void CheckRange()
    {
        if (string.IsNullOrWhiteSpace(Currency) || string.IsNullOrWhiteSpace(StartDate) || string.IsNullOrWhiteSpace(EndDate)) return;
        try
        {
            RangeResults.Clear();
            var results = _client.Client.GetRatesForDateRange(Currency.ToUpper(), StartDate, EndDate);
            foreach (var r in results)
                RangeResults.Add(r);
            Message = $"Loaded {results.Count} rates for {Currency.ToUpper()}";
        }
        catch (Exception ex) { Message = $"Error: {ex.Message}"; }
    }
}