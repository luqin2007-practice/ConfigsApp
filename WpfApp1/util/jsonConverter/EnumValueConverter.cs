using Configs.app;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Configs.util.jsonConverter
{
    public class EnumValueConverter : JsonConverter<EnumValue>
    {
        public static readonly EnumValueConverter Instance = new EnumValueConverter();

        public override EnumValue? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // 字符串
            if (JsonUtils.TryReadString(ref reader, out var s))
            {
                return new EnumValue(s, s, s);
            }

            // 对象
            if (JsonUtils.TryReadObject(ref reader, out var obj)
                // 对象应包含 value 属性
                && obj!.TryGetPropertyValue("value", out var vNode))
            {
                var value = vNode!.ToString();
                var name = obj.TryGetPropertyValue("name", out vNode) ? vNode!.ToString() : value;
                var desc = obj.TryGetPropertyValue("desc", out vNode) ? vNode!.ToString() : name;
                return new EnumValue(value, name, desc);
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, EnumValue value, JsonSerializerOptions options)
        {
            if (value.Value == value.Name && value.Value == value.Desc)
            {
                writer.WriteStringValue(value.Value);
            }
            else
            {
                writer.WriteStartObject();
                writer.WriteString("value", value.Value);
                if (value.Value != value.Name)
                {
                    writer.WriteString("name", value.Name);
                }
                if (value.Desc != value.Name)
                {
                    writer.WriteString("desc", value.Desc);
                }
                writer.WriteEndObject();
            }
        }
    }
}