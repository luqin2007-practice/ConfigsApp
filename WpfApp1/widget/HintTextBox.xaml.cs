using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Configs.widget;

/// <summary>
/// HintTextBox.xaml 的交互逻辑
/// </summary>
public partial class HintTextBox : UserControl
{
    private static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text), typeof(string), typeof(HintTextBox), new PropertyMetadata(default(string)));

    private static readonly DependencyProperty HintProperty = DependencyProperty.Register(
        nameof(Hint), typeof(string), typeof(HintTextBox), new PropertyMetadata(default(string)));

    private static readonly DependencyProperty HintColorProperty = DependencyProperty.Register(
        nameof(HintColor), typeof(Brush), typeof(HintTextBox), new PropertyMetadata(default(Brush)));

    private static readonly DependencyProperty HintBackgroundProperty = DependencyProperty.Register(
        nameof(HintBackground), typeof(Brush), typeof(HintTextBox), new PropertyMetadata(default(Brush)));

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string Hint
    {
        get => (string)GetValue(HintProperty);
        set => SetValue(HintProperty, value);
    }

    public Brush HintColor
    {
        get => (Brush)GetValue(HintColorProperty);
        set => SetValue(HintColorProperty, value);
    }

    public Brush HintBackground
    {
        get => (Brush)GetValue(HintBackgroundProperty);
        set => SetValue(HintBackgroundProperty, value);
    }

    public TextBox ValueBox => ValueTextBox;
    public TextBlock HintBlock => HintTextBlock;

    public HintTextBox()
    {
        InitializeComponent();
    }
}