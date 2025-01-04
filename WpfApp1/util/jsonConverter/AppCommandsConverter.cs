using Configs.app;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Configs.util.jsonConverter
{
    public class AppCommandsConverter(Types types) : JsonConverter<AppCommands>
    {
        public override AppCommands? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonUtils.TryReadObject(ref reader, out var obj) ? ReadFromJson(obj, types) : null;
        }

        public override void Write(Utf8JsonWriter writer, AppCommands value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public static AppCommands? ReadFromJson(JsonNode? node, Types types)
        {
            if (node is not JsonObject)
            {
                return null;
            }

            var app = node["app"]?.GetValue<string>();
            var test = node["test"]?.GetValue<string>();
            var commands = new AppCommands(app)
            {
                Test = test ?? "where {}"
            };

            if (node["commands"] is JsonArray array)
            {
                var subCommands = array
                    .Select(n => CommandConverter.ReadFromJson(n, types))
                    .Where(c => c != null)
                    .Select(n => n!);
                commands.Commands.AddRange(subCommands);
            }

            return commands;
        }
    }
}