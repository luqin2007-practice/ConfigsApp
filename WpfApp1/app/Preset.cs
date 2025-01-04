using Configs.property;

namespace Configs.app;

public class Preset(string name)
{
    public Dictionary<string, string> Properties = new();

    public List<string> PropertyNames = [];

    public string Name { get; set; } = name;

    public string Tooltip => "包含属性：\n" + string.Join("\n", PropertyNames);

    public void AddProperty(string name, Property value)
    {
        AddProperty(name, value.ValueType.ValueToString(value.InputValue!));
    }

    public void AddProperty(string name, string value)
    {
        Properties[name] = value;
        PropertyNames.Add(name);
    }
}