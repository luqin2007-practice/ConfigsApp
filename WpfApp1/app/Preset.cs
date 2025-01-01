namespace Configs.app;

public class Preset
{
    public string Name { get; set; } = "";
    public Dictionary<string, object> Properties = new();
    public List<string> PropertyNames => Properties.Keys.ToList();
    public string Tooltip => "包含属性：\n" + string.Join("\n", PropertyNames);
}