using System.Text.Json;
using System.Text.Json.Nodes;

namespace Configs.app;

public partial class ConfigFile
{
    public void SetProperty<T>(ConfigProperty property, T value)
    {
        switch (Read())
        {
            case JsonNode json:
                _setJsonValue(property, json, value);
                break;
        }
    }

    public void RemoveProperty(ConfigProperty property)
    {
        switch (_file)
        {
            case JsonNode json:
                _removeJsonValue(property, json);
                break;
        }
    }

    private void _setJsonValue<T>(ConfigProperty property, JsonNode json, T value)
    {
        var node = json;
        foreach (var path in _queryPath(property))
        {
            if (node![path] == null) node[path] = new JsonObject();
            node = node[path];
        }

        node![property.Property] = JsonNode.Parse(JsonSerializer.Serialize(value));
    }

    private void _removeJsonValue(ConfigProperty property, JsonNode json)
    {
        var node = json;
        foreach (var path in _queryPath(property))
        {
            if (node![path] == null) return;
            node = node[path];
        }

        if (node is JsonObject obj) obj.Remove(property.Property);
    }
}