using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Configs.app;

namespace Configs.util.jsonConverter;

public class DefaultValueConverter : JsonConverter<DefaultValue>
{
    public static readonly DefaultValueConverter Instance = new DefaultValueConverter();

    public override DefaultValue? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return JsonUtils.TryReadObjectOrString(ref reader, out var n) ? ReadFromJson(n) : null;
    }

    public override void Write(Utf8JsonWriter writer, DefaultValue value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public static DefaultValue? ReadFromJson(JsonNode? node)
    {
        switch (node)
        {
            case JsonValue s:
            {
                var defValue = s.ToString();
                return new DefaultValue(defValue, defValue, defValue);
            }

            case JsonObject obj:
            {
                var windows = obj["windows"]?.ToString();
                var linux = obj["linux"]?.ToString();
                var mac = obj["mac"]?.ToString();
                return new DefaultValue(windows, linux, mac);
            }
            
            default:
                return null;
        }
    }
}