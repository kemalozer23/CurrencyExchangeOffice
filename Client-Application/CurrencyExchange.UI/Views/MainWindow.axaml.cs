using Avalonia.Controls;
using CurrencyExchange.UI.ViewModels;

namespace CurrencyExchange.UI.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}