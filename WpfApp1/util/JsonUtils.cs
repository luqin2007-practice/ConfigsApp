using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Configs.util;

public static class JsonUtils
{
    public static bool TryReadObject(ref Utf8JsonReader reader, [MaybeNullWhen(false)] out JsonObject obj)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            obj = null;
            return false;
        }

        reader.Read();
        obj = new JsonObject();
        var propertyName = "";
        while (true)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.EndObject:
                    reader.Read();
                    return true;
                case JsonTokenType.None:
                    throw new JsonException("未知的 Json None 标记");
                case JsonTokenType.StartObject:
                    TryReadObject(ref reader, out var o);
                    obj[propertyName] = o!;
                    break;
                case JsonTokenType.StartArray:
                    TryReadArray(ref reader, out var arr);
                    obj[propertyName] = arr!;
                    break;
                case JsonTokenType.EndArray:
                    throw new JsonException("未知的 Json 数组结束符");
                case JsonTokenType.PropertyName:
                    propertyName = reader.GetString()!;
                    reader.Read();
                    break;
                case JsonTokenType.Comment:
                    // 跳过注释
                    reader.Read();
                    break;
                case JsonTokenType.String:
                    obj[propertyName] = reader.GetString()!;
                    reader.Read();
                    break;
                case JsonTokenType.Number:
                    obj[propertyName] = reader.GetDouble();
                    reader.Read();
                    break;
                case JsonTokenType.True:
                    obj[propertyName] = true;
                    reader.Read();
                    break;
                case JsonTokenType.False:
                    obj[propertyName] = false;
                    reader.Read();
                    break;
                case JsonTokenType.Null:
                    obj[propertyName] = null;
                    reader.Read();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(reader));
            }
        }
    }

    public static bool TryReadArray(ref Utf8JsonReader reader, [MaybeNullWhen(false)] out JsonArray array)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            array = null;
            return false;
        }

        reader.Read();
        array = [];
        while (true)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.None:
                    throw new JsonException("未知的 Json None 标记");
                case JsonTokenType.StartObject:
                    TryReadObject(ref reader, out var obj);
                    array.Add(obj!);
                    break;
                case JsonTokenType.EndObject:
                    throw new JsonException("未知的 Json 对象结束符");
                case JsonTokenType.StartArray:
                    TryReadArray(ref reader, out var arr);
                    array.Add(arr!);
                    break;
                case JsonTokenType.EndArray:
                    return true;
                case JsonTokenType.PropertyName:
                    throw new JsonException("未知的 Json 属性标识符");
                case JsonTokenType.Comment:
                    // 跳过注释
                    reader.Read();
                    break;
                case JsonTokenType.String:
                    array.Add(reader.GetString()!);
                    reader.Read();
                    break;
                case JsonTokenType.Number:
                    array.Add(reader.GetDouble());
                    reader.Read();
                    break;
                case JsonTokenType.True:
                    array.Add(true);
                    reader.Read();
                    break;
                case JsonTokenType.False:
                    array.Add(false);
                    reader.Read();
                    break;
                case JsonTokenType.Null:
                    array.Add(null);
                    reader.Read();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(reader));
            }
        }
    }

    public static bool TryReadString(ref Utf8JsonReader reader, [MaybeNullWhen(false)] out string s)
    {
        if (reader.TokenType is JsonTokenType.String or JsonTokenType.PropertyName)
        {
            s = reader.GetString()!;
            reader.Read();
            return true;
        }

        s = null;
        return false;
    }

    public static bool TryReadObjectOrString(ref Utf8JsonReader reader, [MaybeNullWhen(false)] out JsonNode n)
    {
        if (TryReadString(ref reader, out var s))
        {
            n = JsonValue.Create(s);
            return true;
        }

        if (TryReadObject(ref reader, out var o))
        {
            n = o;
            return true;
        }

        n = null;
        return false;
    }
}