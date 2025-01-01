using System.Text.Json;
using System.Text.Json.Serialization;
using Configs.app;

namespace Configs.util.jsonConverter;

public partial class TypeConverter(Types types) : JsonConverter<IType>
{
    public override IType? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (JsonUtils.TryReadString(ref reader, out var typeName))
        {
            return types[typeName];
        }

        if (JsonUtils.TryReadObject(ref reader, out var typeObject) && typeObject.ContainsKey("type"))
        {
            return ReadFromJsonObject(typeObject, types);
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, IType value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}