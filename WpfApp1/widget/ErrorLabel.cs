using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Configs.widget;

public class ErrorLabel : Label
{
    public ErrorLabel()
    {
        Visibility = Visibility.Hidden;
        Foreground = new SolidColorBrush(Colors.Red);
    }

    public void Show(string error, string? detail)
    {
        Content = detail == null ? error : $"{error}\n{detail}";
        Visibility = Visibility.Visible;
    }

    public void Hide()
    {
        Visibility = Visibility.Hidden;
        Content = string.Empty;
    }
}