using System.Globalization;
using System.Windows.Data;
using Configs.app;

namespace Configs.util.converter;

[ValueConversion(typeof(EnumType), typeof(List<EnumValue>))]
public class EnumTypeToValuesConverter() : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is EnumType et ? et.Values : [];
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}