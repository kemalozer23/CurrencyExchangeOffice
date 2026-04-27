using CurrencyExchange.UI.Services;

namespace CurrencyExchange.UI.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly ExchangeClient _exchangeClient = new();
    private ViewModelBase _currentView;
    private string _loggedInUser = string.Empty;

    public MainViewModel()
    {
        _currentView = new LoginViewModel(this, _exchangeClient);
    }

    public ViewModelBase CurrentView
    {
        get => _currentView;
        set => SetField(ref _currentView, value);
    }

    public void NavigateToRegister()
        => CurrentView = new RegisterViewModel(this, _exchangeClient);

    public void NavigateToLogin()
        => CurrentView = new LoginViewModel(this, _exchangeClient);

    public void NavigateToDashboard(string username)
    {
        _loggedInUser = username;
        CurrentView = new DashboardViewModel(this, _exchangeClient, username);
    }

    public void NavigateToHistory(string username)
        => CurrentView = new HistoryViewModel(this, _exchangeClient, username);
}