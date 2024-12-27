using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Configs.widget;

namespace Configs.util.converter;

public class AppButtonToNameVisibility : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var b = value as AppButton;
        if (b!.AppIcon == null)
        {
            return Visibility.Visible;
        }

        return string.IsNullOrEmpty(b.AppName) ? Visibility.Hidden : Visibility.Visible;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}