using System.Windows;
using Configs.util;

namespace Configs.widget.property
{
    /// <summary>
    /// BoolPropertyElement.xaml 的交互逻辑
    /// </summary>
    public partial class BoolPropertyElement : BoolPropertyUserControl
    {
        public BoolPropertyElement()
        {
            InitializeComponent();
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            Error = Property.ApplyValue(PropertyValue).ToError("设置失败");
        }
    }
}