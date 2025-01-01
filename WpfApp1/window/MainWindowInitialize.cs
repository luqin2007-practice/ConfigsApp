using System.IO;
using Configs.app;
using Configs.property;
using Configs.util;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Windows.Controls;
using System.Windows;
using Configs.util.jsonConverter;
using TypeConverter = Configs.util.jsonConverter.TypeConverter;

namespace Configs.window;

public partial class MainWindow
{
    private string _loadingHead = "";
    private string _loadingApp = "";

    private static JsonSerializerOptions DefaultOptions => new()
    {
        Converters =
        {
            TypesConverter.Instance, 
            EnumValueConverter.Instance, 
            DefaultValueConverter.Instance, 
            ConfigFileConverter.Instance
        }
    };

    private string LoadingMessage
    {
        set => LoadingItem.Content = value;
    }

    private async Task InitializeProperties(List<string> apps)
    {
        if (apps.Count == 0)
        {
            return;
        }

        Status.Visibility = Visibility.Visible;
        LoadingProg.Maximum = apps.Count;
        foreach (var s in apps)
        {
            LoadingProg.Value++;
            _loadingHead = $"{LoadingProg.Value}/{apps.Count}";
            _loadingApp = Path.GetFileName(s);
            LoadingMessage = $" {_loadingHead} 正在加载 {_loadingApp}";
            await _addApp(s);
        }
        Status.Visibility = Visibility.Hidden;
    }

    private async Task _addApp(string dir)
    {
        // 读 app 和 types
        LoadingMessage = $" {_loadingHead} 正在加载 {_loadingApp} - {FileUtils.AppJson}";
        var appInfo = await FileUtils.ReadJson<AppInformation>(dir, FileUtils.AppJson);
        if (appInfo == null)
        {
            return;
        }
        appInfo.Fix(dir);

        LoadingMessage = $" {_loadingHead} 正在加载 {_loadingApp} - {FileUtils.TypesJson}";
        var types = await FileUtils.ReadJson<Types>(dir, FileUtils.TypesJson, DefaultOptions) ?? Types.Default;
        var options = new JsonSerializerOptions(DefaultOptions);
        options.Converters.Add(new TypeConverter(types));
        var app = new AppPageData(appInfo);
        ViewModel.Apps.Add(app);

        // 读 commands
        LoadingMessage = $" {_loadingHead} 正在加载 {_loadingApp} - {FileUtils.CommandsJson}";
        var commandsJson = await FileUtils.ReadJson<AppCommands>(dir, FileUtils.CommandsJson, options);
        if (commandsJson != null)
        {
            commandsJson.Fix(appInfo);
            _addCommandProperties(app, commandsJson);
            commandsJson.Commands.ForEach(c => c.Fix());
        }

        // 读 files
        LoadingMessage = $" {_loadingHead} 正在加载 {_loadingApp} - {FileUtils.FilesJson}";
        var configsJson = await FileUtils.ReadJson<Dictionary<string, ConfigFile>>(dir, FileUtils.FilesJson, options);
        foreach (var (name, file) in configsJson ?? [])
        {
            LoadingMessage = $" {_loadingHead} 正在加载 {_loadingApp} - {name + FileUtils.FilesDescExt}";
            var desc = await FileUtils.ReadJson<JsonObject>(dir, name + FileUtils.FilesDescExt, options);
            LoadingMessage = $" {_loadingHead} 正在加载 {_loadingApp} - {name + FileUtils.FilesGroupExt}";
            var group = await FileUtils.ReadJson<JsonObject>(dir, name + FileUtils.FilesGroupExt, options);
            file.Fix(name, desc!, group, types);
            _addFileProperty(app, file);
        }
    }

    private void _addCommandProperties(AppPageData page, AppCommands commands)
    {
        commands.Commands.ForEach(c =>
        {
            _addCommandProperty(commands.App, page, c);
        });
    }

    private void _addCommandProperty(string app, AppPageData page, Command command)
    {
        _addSeparator(page.MainPage);
        Property property = command.Type switch
        {
            StringType => new StringCommandProperty(app, command),
            BoolType => new BoolCommandProperty(app, command),
            IntType => throw new NotImplementedException(),
            DirectoryType => throw new NotImplementedException(),
            FileType => throw new NotImplementedException(),
            ListType => throw new NotImplementedException(),
            EnumType => throw new NotImplementedException(),
            ArrayType => throw new NotImplementedException(),
            _ => throw new NotSupportedException($"未知命令类型 {command.Property} : {command.Type.Type}")
        };
        property.InitializeDisplay(page.MainPage);
    }

    private void _addFileProperty(AppPageData app, ConfigFile file)
    {
    }

    /// <summary>
    /// 在属性列表中添加分隔符
    /// </summary>
    private void _addSeparator(StackPanel panel)
    {
        // 跳过第一行并准备插入
        if (panel.Children.Count == 0)
        {
            return;
        }

        // 插入分隔符
        var sp = new Separator()
        {
            Margin = new Thickness(0, 5, 0, 10),
        };
        panel.Children.Add(sp);
    }
}