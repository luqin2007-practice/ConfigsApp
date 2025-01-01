using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Configs.widget;

public class ErrorLabel : Label, INotifyPropertyChanged
{
    private static readonly DependencyProperty ErrorProperty = DependencyProperty.Register(
        nameof(Error), typeof(Error), typeof(ErrorLabel), new PropertyMetadata(null, OnErrorChanged));

    public Error? Error
    {
        get => (Error)GetValue(ErrorProperty); 
        set => SetValue(ErrorProperty, value);
    }

    public ErrorLabel()
    {
        Visibility = Visibility.Collapsed;
        Foreground = new SolidColorBrush(Colors.Red);
    }

    private static void OnErrorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var label = (ErrorLabel) d;
        var error = label.Error;

        if (error == null)
        {
            label.Visibility = Visibility.Collapsed;
        }
        else
        {
            label.Visibility = Visibility.Visible;
            label.Content = $"{error.Name}{(error.Desc == null ? string.Empty : $"\n{error.Desc}")}";
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public record Error(string Name, string? Desc)
{
    public static implicit operator Error(string value) => new Error(value, null);
    public static implicit operator Error((string, string?) values) => new Error(values.Item1, values.Item2);
}