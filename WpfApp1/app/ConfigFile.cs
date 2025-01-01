namespace Configs.app;

public partial record ConfigFile(string Type, List<string> Paths)
{
    public string SelectedPath { get; set; } = Paths[0];

    public string Name { get; set; } = "Unknown";

    public ConfigGroup Root { get; set; } = new("Root", "Root", "Root", [], []);
};

public record ConfigGroup(string Group, string Name, string Desc, List<ConfigProperty> Properties, List<ConfigGroup> Groups);

public record ConfigProperty(string Property, string Name, string Desc, IType Type, DefaultValue Default, bool Hidden);