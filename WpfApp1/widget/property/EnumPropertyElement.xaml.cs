using Configs.util;
using System.Windows.Controls;

namespace Configs.widget.property
{
    /// <summary>
    /// EnumPropertyElement.xaml 的交互逻辑
    /// </summary>
    public partial class EnumPropertyElement : EnumPropertyUserControl
    {
        public EnumPropertyElement()
        {
            InitializeComponent();
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Error = Property.ApplyValue(PropertyValue).ToError("设置失败");
        }
    }
}