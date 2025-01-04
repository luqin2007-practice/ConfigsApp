using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Configs.util.converter
{
    public class StringToVisibilityConverter(string target) : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return string.Equals(target, value as string) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StringToVisibilityConverterExt : MarkupExtension
    {
        public string Target { get; set; }
        public override object? ProvideValue(IServiceProvider serviceProvider)
        {
            return new StringToVisibilityConverter(Target);
        }
    }
}