using System.Windows;
using Configs.util;

namespace Configs.widget.property;

/// <summary>
/// StringProperty.xaml 的交互逻辑
/// </summary>
public partial class StringPropertyElement : StringPropertyUserControl
{
    public StringPropertyElement()
    {
        InitializeComponent();
    }

    private void UIElement_OnLostFocus(object sender, RoutedEventArgs e)
    {
        var value = (sender as HintTextBox)!.ValueTextBox.Text;
        Error = Property.ApplyValue(value).ToError("设置失败");
    }
}