using System.Windows;
using System.Windows.Controls;
using Configs.property;
using Configs.util;

namespace Configs.widget.property
{
    /// <summary>
    /// IntPropertyElement.xaml 的交互逻辑
    /// </summary>
    public partial class IntPropertyElement : IntPropertyUserControl
    {
        public IntPropertyElement()
        {
            InitializeComponent();
        }

        private void UIElement_OnLostFocus(object sender, RoutedEventArgs e)
        {
            Error = Property.ApplyValue(PropertyValue).ToError("设置失败");
        }
    }
}