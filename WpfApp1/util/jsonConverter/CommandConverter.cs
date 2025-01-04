using Configs.app;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Configs.util.jsonConverter
{
    public class CommandConverter(Types types) : JsonConverter<Command>
    {
        public override Command? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonUtils.TryReadObject(ref reader, out var obj) ? ReadFromJson(obj, types) : null;
        }

        public override void Write(Utf8JsonWriter writer, Command value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public static Command? ReadFromJson(JsonNode? node, Types types)
        {
            if (node is JsonObject obj)
            {
                var property = obj["property"]!.GetValue<string>();
                var name = obj["name"]?.GetValue<string>() ?? property;
                var desc = obj["desc"]?.GetValue<string>() ?? name;
                var type = TypeConverter.ReadType(obj["type"], types) ?? StringType.Default;
                var command = ReadCommandDetail(obj["override"]);
                var defValue = DefaultValueConverter.ReadFromJson(obj["default"]) ?? new DefaultValue();
                return new Command
                {
                    Property = property,
                    Name = name,
                    Desc = desc,
                    Type = type,
                    Commands = command,
                    Default = defValue
                };
            }

            return null;
        }

        public static CommandDetail ReadCommandDetail(JsonNode? node)
        {
            var read = node?["read"]?.GetValue<string>();
            var write = node?["write"]?.GetValue<string>();
            var revoke = node?["revoke"]?.GetValue<string>();
            return new CommandDetail(read, write, revoke);
        }
    }
}