using System.Text.Json;
using System.Text.Json.Serialization;
using Configs.app;

namespace Configs.util.jsonConverter;

public partial class TypeConverter(Types types) : JsonConverter<IType>
{
    public Types Types { get; } = types;

    public override IType? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (JsonUtils.TryReadString(ref reader, out var typeName))
        {
            return Types[typeName];
        }

        if (JsonUtils.TryReadObject(ref reader, out var typeObject) && typeObject.ContainsKey("type"))
        {
            return ReadType(typeObject, Types);
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, IType value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}