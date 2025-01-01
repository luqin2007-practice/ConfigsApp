using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Configs.window
{
    /// <summary>
    /// AddPresets.xaml 的交互逻辑
    /// </summary>
    public partial class AddPresets : Window, INotifyPropertyChanged
    {
        public List<string> Properties { get; set; } = [];
        private MainWindow _parent;

        public AddPresets(MainWindow parent)
        {
            _parent = parent;
            InitializeComponent();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var list = List.SelectedItems.Cast<string>().ToList();
            var error = _parent.AddPresets(PresetName.Text, list);

            if (error != null)
            {
                MessageBox.Show(error);
            }
            else
            {
                Close();
            }
        }
    }
}
