using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Configs.widget;

/// <summary>
/// AppButton.xaml 的交互逻辑
/// </summary>
public partial class AppButton : UserControl
{
    public static readonly DependencyProperty AppNameProperty = DependencyProperty.Register(
        nameof(AppName), typeof(string), typeof(AppButton), new PropertyMetadata(default(string)));

    public static readonly DependencyProperty AppIconProperty = DependencyProperty.Register(
        nameof(AppIcon), typeof(ImageSource), typeof(AppButton), new PropertyMetadata(default(ImageSource)));

    public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent(
        nameof(Click), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AppButton));

    /// <summary>
    /// 应用名
    /// </summary>
    public string? AppName
    {
        get => (string)GetValue(AppNameProperty);
        set => SetValue(AppNameProperty, value);
    }

    /// <summary>
    /// 应用图标
    /// </summary>
    public ImageSource? AppIcon
    {
        get => (ImageSource)GetValue(AppIconProperty);
        set => SetValue(AppIconProperty, value);
    }

    public event RoutedEventHandler Click
    {
        add => AddHandler(ClickEvent, value);
        remove => RemoveHandler(ClickEvent, value);
    }

    public AppButton()
    {
        InitializeComponent();
        AButton.Click += (_, _) => RaiseEvent(new RoutedEventArgs(ClickEvent));
    }
}