using System.Globalization;
using System.Windows.Data;

namespace Configs.util.converter;

public class AppButtonToNameContent : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return string.IsNullOrEmpty(value as string) ? "未知应用" : value as string;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}