namespace Configs.app;

public partial record ConfigFile(string Type, List<string> Paths)
{
    public string SelectedPath { get; set; } = Environment.ExpandEnvironmentVariables(Paths[0]);

    public string Name { get; set; } = "Unknown";

    public ConfigGroup Root { get; set; } = new("Root", "Root", "Root", [], [], null);

    private List<string> queryPath(ConfigProperty property)
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
};

public interface IConfigElement
{
    ConfigGroup? Parent { get; }
}

public record ConfigGroup(string Group, string Name, string Desc, List<ConfigProperty> Properties, List<ConfigGroup> Groups, ConfigGroup? Parent) : IConfigElement;

public record ConfigProperty(string Property, string Name, string Desc, IType Type, DefaultValue Default, bool Hidden, ConfigGroup? Parent) : IConfigElement;