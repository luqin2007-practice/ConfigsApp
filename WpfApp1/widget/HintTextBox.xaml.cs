using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Configs.widget
{
    /// <summary>
    /// HintTextBox.xaml 的交互逻辑
    /// </summary>
    public partial class HintTextBox : UserControl, INotifyPropertyChanged
    {
        private static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(HintTextBox), new PropertyMetadata(default(string)));
        private static readonly DependencyProperty HintProperty = DependencyProperty.Register(nameof(Hint), typeof(string), typeof(HintTextBox), new PropertyMetadata(default(string)));
        private static readonly DependencyProperty HintColorProperty = DependencyProperty.Register(nameof(HintColor), typeof(Brush), typeof(HintTextBox), new PropertyMetadata(Brushes.Gray));
        private static readonly DependencyProperty HintBackgroundProperty = DependencyProperty.Register(nameof(HintBackground), typeof(Brush), typeof(HintTextBox), new PropertyMetadata(default(Brush)));

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

        private bool _isValueChanged;

        public HintTextBox()
        {
            InitializeComponent();
        }

        private void ValueTextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (_isValueChanged)
            {
                RaiseEvent(new RoutedEventArgs(LostFocusEvent, sender));
                _isValueChanged = false;
            }
        }

        private void ValueTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _isValueChanged = true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}