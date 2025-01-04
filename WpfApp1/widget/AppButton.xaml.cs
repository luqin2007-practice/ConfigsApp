using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static System.Windows.DependencyProperty;

namespace Configs.widget
{
    /// <summary>
    /// AppButton.xaml 的交互逻辑
    /// </summary>
    public partial class AppButton : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty AppNameProperty = Register(nameof(AppName), typeof(string), typeof(AppButton), new PropertyMetadata(default(string))), AppIconProperty = Register(nameof(AppIcon), typeof(ImageSource), typeof(AppButton), new PropertyMetadata(default(ImageSource)));

        static AppButton()
        {
        }

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
            get => (ImageSource?)GetValue(AppIconProperty);
            set => SetValue(AppIconProperty, value);
        }

        public AppButton()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}