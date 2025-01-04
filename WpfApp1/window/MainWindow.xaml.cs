using Configs.app;
using Configs.property;
using Configs.util;
using Configs.widget;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Configs.window
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// ReSharper disable once UnusedMember.Global
    /// </summary>
    public partial class MainWindow
    {
        public MainWindowViewModel ViewModel { get; set; } = new MainWindowViewModel();

        public MainWindow()
        {
            // 初始化默认页面
            InitializeComponent();
            // 添加控件
            Loaded += async (_, _) =>
            {
                var apps = FileUtils.GetConfigList();
                await InitializeProperties(apps);
            };
        }
    }

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private static AppPageData? _helloApp;

        public static AppPageData HelloApp => _helloApp ??= AppPageData.CreateHelloPage();

        public ObservableCollection<AppPageData> Apps { get; } = [HelloApp];

        private AppPageData _selectedApp = HelloApp;

        public AppPageData SelectedApp
        {
            get => _selectedApp;
            set
            {
                _selectedApp = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class AppPageData(AppInformation app) : INotifyPropertyChanged
    {
        public AppInformation App { get; set; } = app;

        public Visibility Display { get; set; } = Visibility.Visible;

        public StackPanel MainPage { get; set; } = new();

        public Visibility HasPresets => App.HasPresets ? Visibility.Visible : Visibility.Collapsed;

        public ImageSource? AppIcon { get; set; }

        public ObservableCollection<Preset> Presets { get; set; } = [];

        public Dictionary<string, Preset> PresetMap = [];

        public ObservableCollection<string> Properties = [];

        public Dictionary<string, Property> PropertyMap = [];

        public static AppPageData CreateHelloPage()
        {
            var app = new AppInformation
            {
                Name = "说明",
                HasPresets = false
            };
            var page = new HelloPage();
            var data = new AppPageData(app);
            data.MainPage.Children.Add(page);
            data.Display = Visibility.Collapsed;
            return data;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}