using System;
using Avalonia.Data.Converters;
using Avalonia.Media;
using System.Globalization;

namespace CurrencyExchange.UI.Converters;

public class TypeToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value?.ToString() switch
        {
            "BUY" => new SolidColorBrush(Color.Parse("#a6e3a1")),
            "SELL" => new SolidColorBrush(Color.Parse("#fab387")),
            "TOPUP" => new SolidColorBrush(Color.Parse("#89b4fa")),
            _ => new SolidColorBrush(Color.Parse("#888"))
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}