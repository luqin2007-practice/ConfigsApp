﻿using Configs.app;
using Configs.property;
using Configs.util;
using Configs.util.jsonConverter;
using Microsoft.Win32;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows;
using System.Windows.Controls;
using CommandConverter = Configs.util.jsonConverter.CommandConverter;

namespace Configs.window
{
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
            options.Converters.Add(new CommandConverter(types));
            options.Converters.Add(new AppCommandsConverter(types));
            var app = new AppPageData(appInfo)
            {
                AppIcon = FileUtils.LoadImageSource(dir, appInfo.Icon)
            };
            ViewModel.Apps.Add(app);

            // 读 commands
            LoadingMessage = $" {_loadingHead} 正在加载 {_loadingApp} - {FileUtils.CommandsJson}";
            var commandsJson = await FileUtils.ReadJson<AppCommands>(dir, FileUtils.CommandsJson, options);
            if (commandsJson != null)
            {
                commandsJson.Fix(appInfo);
                commandsJson.Commands.ForEach(c => c.Fix());
                // 检查并修复应用位置
                var commandPath = _checkAppCommand(appInfo.Name, commandsJson.App, commandsJson.Test);
                if (commandPath != null)
                {
                    commandsJson.App = commandPath;
                    _addCommandProperties(app, commandsJson);
                }
            }

            // 读 files
            LoadingMessage = $" {_loadingHead} 正在加载 {_loadingApp} - {FileUtils.FilesJson}";
            // var configsJson = await FileUtils.ReadJson<Dictionary<string, ConfigFile>>(dir, FileUtils.FilesJson, options);
            var configFilesPath = Path.Join(dir, FileUtils.FilesJson);
            Dictionary<string, ConfigFile>? configsJson = null;
            if (File.Exists(configFilesPath))
            {
                await using var configsJsonFile = File.OpenRead(Path.Join(dir, FileUtils.FilesJson));
                var configsJsonStr = await JsonNode.ParseAsync(configsJsonFile);
                configsJson = ConfigFilesConverter.ReadFromJson(configsJsonStr);
            }
            foreach (var (name, file) in configsJson ?? [])
            {
                LoadingMessage = $" {_loadingHead} 正在加载 {_loadingApp} - {name + FileUtils.FilesDescExt}";
                var desc = await FileUtils.ReadJson<JsonObject>(dir, name + FileUtils.FilesDescExt, options);
                LoadingMessage = $" {_loadingHead} 正在加载 {_loadingApp} - {name + FileUtils.FilesGroupExt}";
                var group = await FileUtils.ReadJson<JsonObject>(dir, name + FileUtils.FilesGroupExt, options);
                file.Fix(name, desc!, group, types);
                if (_checkAndSelectFilePath(appInfo.Name, file))
                {
                    _addFileProperty(app, file, file.Root);
                }
            }

            // 加载预设列表
            foreach (var preset in Database.Instance.ReadPresets(appInfo.Name))
            {
                app.Presets.Add(preset);
                app.PresetMap[preset.Name] = preset;
            }
        }

        private string? _checkAppCommand(string app, string defaultPath, string test)
        {
            // 尝试数据库中的应用位置
            var path = Database.Instance.ReadAppCommand(app);
            if (path != null && CommandUtils.Execute(test, path).IsOk)
            {
                return path;
            }
            // 尝试使用默认位置
            if (CommandUtils.Execute(test, defaultPath).IsOk)
            {
                return defaultPath;
            }
            // 失败 手动选择位置
            OpenFileDialog dialog = new()
            {
                Title = $"选择 {app} 位置",
                Filter = "可执行文件|*.exe"
            };
            if (dialog.ShowDialog() == true && CommandUtils.Execute(test, dialog.FileName).IsOk)
            {
                Database.Instance.SaveAppCommand(app, dialog.FileName);
                return dialog.FileName;
            }

            return null;
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
                EnumType => new EnumCommandProperty(app, command),
                ArrayType => throw new NotImplementedException(),
                _ => throw new NotSupportedException($"未知命令类型 {command.Property} : {command.Type.Type}")
            };
            page.Properties.Add(property.PropertyName);
            page.PropertyMap[property.PropertyName] = property;
            property.InitializeDisplay(page.MainPage);
        }

        private bool _checkAndSelectFilePath(string app, ConfigFile file)
        {
            // 尝试数据库中的文件位置
            var path = Database.Instance.ReadAppConfig(app, file.Name);
            if (path != null && File.Exists(path))
            {
                file.SelectedPath = path;
                return true;
            }
            // 尝试使用默认位置
            foreach (var p in file.Paths)
            {
                path = Environment.ExpandEnvironmentVariables(p);
                if (File.Exists(path))
                {
                    file.SelectedPath = path;
                    return true;
                }
            }
            // 失败 手动选择位置
            OpenFileDialog dialog = new()
            {
                Title = $"选择 {file.Name} 位置",
                Filter = file.Type switch
                {
                    "json" => "JSON 文件|*.json",
                    _ => "所有文件|*.*"
                }
            };
            if (dialog.ShowDialog() == true && File.Exists(dialog.FileName))
            {
                Database.Instance.SaveAppConfig(app, file.Name, dialog.FileName);
                file.SelectedPath = dialog.FileName;
                return true;
            }

            return false;
        }

        private void _addFileProperty(AppPageData page, ConfigFile file, ConfigGroup group)
        {
            _addSeparator(page.MainPage);
            foreach (var property in group.Properties)
            {
                Property prop = property.Type switch
                {
                    StringType => new StringFileProperty(file, property),
                    BoolType => new BoolFileProperty(file, property),
                    IntType => new IntFileProperty(file, property),
                    DirectoryType => new StringFileProperty(file, property), // TODO
                    FileType => new StringFileProperty(file, property), // TODO
                    ListType => throw new NotImplementedException(),
                    EnumType => new EnumFileProperty(file, property),
                    ArrayType => throw new NotImplementedException(),
                    _ => throw new NotSupportedException($"未知属性类型 {property.Property} : {property.Type.Type}")
                };
                page.Properties.Add(prop.PropertyName);
                page.PropertyMap[prop.PropertyName] = prop;
                prop.InitializeDisplay(page.MainPage);
            }
            group.Groups.ForEach(g => _addFileProperty(page, file, g));
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
}