using Configs.property;

namespace Configs.app;

public class Preset
{
    public string Name { get; set; } = "";

    public Dictionary<string, object> Properties = [];
    
    public List<string> PropertyNames = [];
    
    public string Tooltip => "包含属性：\n" + string.Join("\n", PropertyNames);

    public void AddProperty(string name, Property value)
    {
        Properties[name] = value.InputValue!;
        PropertyNames.Add(name);
    }
}