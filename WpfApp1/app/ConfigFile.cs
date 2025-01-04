namespace Configs.app;

public partial class ConfigFile(string type, List<string> paths)
{
    public string SelectedPath { get; set; } = Environment.ExpandEnvironmentVariables(paths[0]);

    public string Name { get; set; } = "Unknown";

    public ConfigGroup Root { get; set; } = new("Root", "Root", "Root", [], [], null);

    public string Type { get; init; } = type;
    public List<string> Paths { get; init; } = paths;

    private List<string> _queryPath(ConfigProperty property)
    {
        List<string> path = [];
        // 查询路径
        IConfigElement? p = property;
        while (p.Parent != null)
        {
            path.Insert(0, p.Parent.Group);
            p = p.Parent;
        }

        return path;
    }
}

public interface IConfigElement
{
    ConfigGroup? Parent { get; }
}

public class ConfigGroup(string group, string name, string desc, List<ConfigProperty> properties, List<ConfigGroup> groups, ConfigGroup? parent) : IConfigElement
{
    public string Group { get; init; } = group;
    public string Name { get; init; } = name;
    public string Desc { get; init; } = desc;
    public List<ConfigProperty> Properties { get; init; } = properties;
    public List<ConfigGroup> Groups { get; init; } = groups;
    public ConfigGroup? Parent { get; init; } = parent;
}

public class ConfigProperty(string property, string name, string desc, IType type, DefaultValue @default, bool hidden, ConfigGroup? parent) : IConfigElement
{
    public string Property { get; init; } = property;
    public string Name { get; init; } = name;
    public string Desc { get; init; } = desc;
    public IType Type { get; init; } = type;
    public DefaultValue Default { get; init; } = @default;
    public bool Hidden { get; init; } = hidden;
    public ConfigGroup? Parent { get; init; } = parent;
}