using Configs.app;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Configs.util.jsonConverter
{
    /*
     * {
     *   <TypeName>: [ <EnumType>... ]
     * }
     */
    public class TypesConverter : JsonConverter<Types>
    {
        public static readonly TypesConverter Instance = new TypesConverter();

        public override Types? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                return null;
            }

            // Object Start
            reader.Read();

            var types = new Types();
            while (reader.TokenType == JsonTokenType.PropertyName)
            {
                JsonUtils.TryReadString(ref reader, out var typeName);

                // Array Start
                reader.Read();

                List<EnumValue> typeValues = [];
                while (reader.TokenType != JsonTokenType.EndArray)
                {
                    typeValues.Add(EnumValueConverter.Instance.Read(ref reader, typeof(EnumValue), options)!);
                }
                types.Add(new EnumType(typeName!, typeValues));

                // Array End
                reader.Read();
            }

            return types;
        }

        public override void Write(Utf8JsonWriter writer, Types types, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            foreach (var type in types)
            {
                if (type is not EnumType et)
                {
                    throw new JsonException($"无法序列化类型 {type.Type}");
                }

                writer.WritePropertyName(type.Type);
                writer.WriteStartArray();
                foreach (var value in et.Values)
                {
                    EnumValueConverter.Instance.Write(writer, value, options);
                }
                writer.WriteEndArray();
            }
            writer.WriteEndObject();
        }
    }
}