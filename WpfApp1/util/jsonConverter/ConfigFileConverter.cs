using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Configs.app;

namespace Configs.util.jsonConverter;

public class ConfigFileConverter : JsonConverter<ConfigFile>
{
    public static readonly ConfigFileConverter Instance = new();

    public override ConfigFile? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (JsonUtils.TryReadString(ref reader, out var filePath))
        {
            filePath = Environment.ExpandEnvironmentVariables(filePath);
            var type = Path.GetExtension(filePath);
            return new ConfigFile(type, [filePath]);
        }

        if (JsonUtils.TryReadArray(ref reader, out var array))
        {
            var list = array.Select(n => n!.GetValue<string>()).ToList();
            var type = Path.GetExtension(list[0]);
            return new ConfigFile(type, list);
        }

        if (JsonUtils.TryReadObject(ref reader, out var obj))
        {
            var list = (obj["file"] as JsonArray)!.Select(n => n!.GetValue<string>()).ToList();
            var type = obj["type"]?.GetValue<string>() ?? Path.GetExtension(list[0]);
            return new ConfigFile(type, list);
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, ConfigFile value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}