using Configs.app;
using System.Windows;

namespace Configs.window;

public partial class MainWindow
{

    private void AddPreset_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new AddPresets(this);
        dialog.Properties.AddRange(ViewModel.SelectedApp.Properties);
        dialog.ShowDialog();
    }

    private void RemovePreset_Click(object sender, RoutedEventArgs e)
    {
        if (PresetListBox.SelectedItem is Preset preset)
        {
            ViewModel.SelectedApp.Presets.Remove(preset);
            ViewModel.SelectedApp.PresetMap.Remove(preset.Name);
        }
    }

    public string? AddPresets(string name, List<string> properties)
    {
        if (ViewModel.SelectedApp.PresetMap.ContainsKey(name))
        {
            return "预设名已存在";
        }

        if (string.IsNullOrEmpty(name))
        {
            return "预设名不能为空";
        }

        if (properties.Count == 0)
        {
            return "无属性";
        }

        var preset = new Preset { Name = name };
        foreach (var property in properties)
        {
            preset.AddProperty(property, ViewModel.SelectedApp.PropertyMap[property]);
        }
        ViewModel.SelectedApp.Presets.Add(preset);
        ViewModel.SelectedApp.PresetMap[name] = preset;
        return null;
    }
}