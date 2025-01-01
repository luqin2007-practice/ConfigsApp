using System.Text.Json.Nodes;

namespace Configs.app;

public partial record ConfigFile
{
    public T GetProperty<T>(ConfigProperty property)
    {
        switch (Read())
        {
            case JsonNode json:
                return getJsonValue<T>(property, json);
        }
        return property.Default.GetValue<T>();
    }
    
    private T getJsonValue<T>(ConfigProperty property, JsonNode json)
    {
        var node = json;
        foreach (var path in queryPath(property))
        {
            node = node[path];
            if (node == null)
            {
                return property.Default.GetValue<T>();
            }
        }

        return node[property.Property] != null
            ? node[property.Property]!.GetValue<T>()!
            : property.Default.GetValue<T>();
    }
}