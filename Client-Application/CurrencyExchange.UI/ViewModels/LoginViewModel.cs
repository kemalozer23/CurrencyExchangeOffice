using System;
using System.Windows.Input;
using CurrencyExchange.UI.Services;

namespace CurrencyExchange.UI.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private readonly MainViewModel _main;
    private readonly ExchangeClient _client;
    private string _username = string.Empty;
    private string _password = string.Empty;
    private string _message = string.Empty;

    public LoginViewModel(MainViewModel main, ExchangeClient client)
    {
        _main = main;
        _client = client;
        LoginCommand = new RelayCommand(Login);
        GoToRegisterCommand = new RelayCommand(() => _main.NavigateToRegister());
    }

    public string Username { get => _username; set => SetField(ref _username, value); }
    public string Password { get => _password; set => SetField(ref _password, value); }
    public string Message { get => _message; set => SetField(ref _message, value); }

    public ICommand LoginCommand { get; }
    public ICommand GoToRegisterCommand { get; }

    private void Login()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            Message = "Please enter username and password.";
            return;
        }

        try
        {
            var result = _client.Client.Login(Username, Password);
            if (result.Success)
                _main.NavigateToDashboard(Username);
            else
                Message = result.Message;
        }
        catch (Exception ex)
        {
            Message = $"Service error: {ex.Message}";
        }
    }
}