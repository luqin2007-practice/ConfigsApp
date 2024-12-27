using System.Windows.Controls;
using Configs.property;

namespace Configs.util;

public class App
{
    public string Name = "";
    public string Icon = "";
    public bool HasPresets = true;

    public List<Preset> __Presets = [];
    public Grid __MainPage = new();
    public List<Property> __Properties = [];
    public Dictionary<string, Property> __PropertyMap = new();
}