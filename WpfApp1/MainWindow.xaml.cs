using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Configs.property;
using Configs.util;
using Configs.widget;
using AppContext = Configs.app.AppContext;

namespace Configs;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public List<util.App> Apps { get; } = [];

    public MainWindow()
    {
        // 初始化默认页面
        Apps.Add(new util.App
        {
            Name = "首页",
            HasPresets = false,
        });
        InitializeComponent();
        // 添加控件
        FileUtils.GetConfigList().ForEach(AddApp);
    }

    private void AddApp(string dir)
    {
        // 读取配置文件
        var config = FileUtils.ReadConfig(dir);
        // 创建属性界面
        var grid = new Grid
        {
            Name = config.Name
        };

        grid.ColumnDefinitions.Clear();
        grid.ColumnDefinitions.Add(new ColumnDefinition
        {
            Width = new GridLength(1, GridUnitType.Star)
        });
        grid.ColumnDefinitions.Add(new ColumnDefinition
        {
            Width = new GridLength(4, GridUnitType.Star)
        });
        if ("git".Equals(config.Name, StringComparison.OrdinalIgnoreCase))
        {
            AddGit(grid);
        }
        _pages.Add(grid);
        // 创建切换按钮
        var button = new AppButton
        {
            AppIcon = FileUtils.ReadImage(dir, config.Icon),
            AppName = config.Name
        };
        button.Click += (_, _) =>
        {
            foreach (var page in _pages)
            {
                page.Visibility = page == grid ? Visibility.Visible : Visibility.Collapsed;
            }
        };
    }

    private void AddGit(Grid grid)
    {
        // var context = new GitAppContext();
        // var configs = context.ExecuteCommand("git config --global --list");
        // if (!configs.IsOk)
        // {
        //     MessageBox.Show("Git: 获取属性失败：" + configs.ErrorMessage);
        //     return;
        // }
        //
        // foreach (var config in configs.OutputMessage.Split('\n'))
        // {
        //     // 收集属性
        //     var eq = config.IndexOf('=');
        //     if (eq == -1) continue;
        //     var key = config[..eq];
        //     // 添加行
        //     AddSeparator();
        //     // 添加属性
        //     var name = key
        //         .Split('.')
        //         .Select(s => char.ToUpper(s[0]) + s[1..])
        //         .Aggregate("", (s1, s2) => s1 + s2);
        //     AddProperty(new GitStringProperty(context, name, key));
        //     // 别名属性
        //     // alias.<alias-name>,<alias-name> 别名,string,,Git 命令别名
        // }
    }

    /// <summary>
    /// 在属性列表中添加分隔符
    /// </summary>
    private void AddSeparator()
    {
        // 跳过第一行并准备插入
        var row = IndexPage.RowDefinitions.Count;
        if (row == 0) return;
        IndexPage.RowDefinitions.Add(new RowDefinition()
        {
            Height = GridLength.Auto
        });

        // 插入分隔符
        var sp = new Separator()
        {
            Margin = new Thickness(0, 5, 0, 10),
        };
        IndexPage.Children.Add(sp);
        Grid.SetRow(sp, row);
        Grid.SetColumn(sp, 0);
        Grid.SetColumnSpan(sp, 2);
    }

    /// <summary>
    /// 在属性列表中添加属性
    /// </summary>
    /// <param name="property">属性</param>
    private void AddProperty<TApp, TValue>(Property<TApp, TValue> property) where TApp : AppContext
    {
        var row = IndexPage.RowDefinitions.Count;
        IndexPage.RowDefinitions.Add(new RowDefinition()
        {
            Height = GridLength.Auto
        });
        property.InitializeDisplay(IndexPage, row);
        property.IsVisible = property.IsVisible;
    }
}