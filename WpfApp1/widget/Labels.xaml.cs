using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Configs.widget;

/// <summary>
/// Labels.xaml 的交互逻辑
/// </summary>
public partial class Labels : UserControl
{
    public static readonly DependencyProperty TextsProperty = DependencyProperty.Register(
        nameof(Texts), typeof(IEnumerable<LabelText>), typeof(Labels),
        new PropertyMetadata(new List<LabelText>()));

    public IEnumerable<LabelText> Texts
    {
        get => (IEnumerable<LabelText>)GetValue(TextsProperty);
        set => SetValue(TextsProperty, value);
    }

    public Labels()
    {
        InitializeComponent();
    }
}

public record LabelText(string Text, Brush Color)
{
    public static implicit operator LabelText(string text) => new(text, Brushes.Black);
    public static implicit operator LabelText((string, Brush) values) => new(values.Item1, values.Item2);
}