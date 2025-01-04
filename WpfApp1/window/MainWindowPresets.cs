using Configs.app;
using Configs.util;
using System.Windows;
using System.Windows.Input;

namespace Configs.window
{
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

        private void PresetListBox_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (PresetListBox.SelectedItem is Preset preset)
            {
                foreach (var (name, value) in preset.Properties)
                {
                    var property = ViewModel.SelectedApp.PropertyMap[name];
                    property.ApplyValue(property.ValueType.StringToValue(value));
                    property.ReloadValue();
                }
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

            var preset = new Preset(name);
            foreach (var property in properties)
            {
                preset.AddProperty(property, ViewModel.SelectedApp.PropertyMap[property]);
            }
            ViewModel.SelectedApp.Presets.Add(preset);
            ViewModel.SelectedApp.PresetMap[name] = preset;
            Database.Instance.SavePreset(ViewModel.SelectedApp.App.Name, ref preset);
            return null;
        }
    }
}