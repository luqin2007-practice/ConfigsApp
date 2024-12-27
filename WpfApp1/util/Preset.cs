namespace Configs.util;

public class Preset
{
    public string Name;
    public Dictionary<string, object> Properties;
    public List<string> PropertyNames => Properties.Keys.ToList();
    public string Tooltip => "包含属性：\n" + string.Join("\n", PropertyNames);
}