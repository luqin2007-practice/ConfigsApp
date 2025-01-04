using Configs.app;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Configs.util.jsonConverter
{
    public partial class TypeConverter
    {
        private static readonly Dictionary<string, Func<JsonNode, Types, IType>> Readers = new Dictionary<string, Func<JsonNode, Types, IType>>
        {
            ["string"] = _readStringType,
            ["bool"] = _readBoolType,
            ["boolean"] = _readBoolType,
            ["int"] = _readIntType,
            ["integer"] = _readIntType,
            ["long"] = _readIntType,
            ["directory"] = _readDirectoryType,
            ["file"] = _readFileType,
            ["list"] = _readListType,
            ["enum"] = _readEnumType,
            ["array"] = _readArrayType,
        };

        public static IType? ReadType(JsonNode? obj, Types types)
        {
            switch (obj)
            {
                // 直接使用已有类型
                case JsonValue:
                    return types[obj.GetValue<string>()];

                // 读取类型
                case JsonObject:
                    {
                        var typeName = obj["type"]!.GetValue<string>();
                        return Readers[typeName](obj, types);
                    }

                default:
                    return null;
            }
        }

        private static StringType _readStringType(JsonNode obj, Types types)
        {
            var regex = obj["regex"]?.GetValue<string>();
            return regex == null ? StringType.Default : new StringType(regex);
        }

        private static BoolType _readBoolType(JsonNode obj, Types types)
        {
            var trueValue = obj["true"]?.GetValue<string>();
            var falseValue = obj["false"]?.GetValue<string>();
            return trueValue == null && falseValue == null
                ? BoolType.Default
                : new BoolType(trueValue ?? "true", falseValue ?? "false");
        }

        private static IntType _readIntType(JsonNode obj, Types types)
        {
            var min = obj["min"]?.GetValue<long>();
            var max = obj["max"]?.GetValue<long>();
            return min == null && max == null
                ? IntType.Default
                : new IntType(min, max);
        }

        private static DirectoryType _readDirectoryType(JsonNode obj, Types types)
        {
            var exist = obj["exist"]?.GetValue<bool>() ?? false;
            return exist ? DirectoryType.MustExist : DirectoryType.Default;
        }

        private static FileType _readFileType(JsonNode obj, Types types)
        {
            var exist = obj["exist"]?.GetValue<bool>() ?? false;
            var ext = obj["ext"]?.GetValue<List<string>>();
            return (exist || (ext != null && ext.Count != 0))
                ? new FileType(exist, ext)
                : FileType.Default;
        }

        private static ListType _readListType(JsonNode obj, Types types)
        {
            var split = obj["split"]?.GetValue<string>() ?? "\n";
            var elementObject = obj["elementType"];
            var elementType = ReadType(elementObject, types) ?? StringType.Default;
            return new ListType(split, elementType);
        }

        private static EnumType _readEnumType(JsonNode obj, Types types)
        {
            var values = (obj["values"] as JsonArray)!
                .Select(value => _readEnumValue(value!))
                .ToList();
            return new EnumType("enum", values);
        }

        private static EnumValue _readEnumValue(JsonNode v)
        {
            switch (v)
            {
                case JsonValue:
                    {
                        var value = v.GetValue<string>();
                        return new EnumValue(value, value, value);
                    }
                case JsonObject valueObj when valueObj.ContainsKey("value"):
                    {
                        var value = valueObj["value"]!.GetValue<string>();
                        var name = valueObj["name"]?.GetValue<string>() ?? value;
                        var desc = valueObj["desc"]?.GetValue<string>() ?? name;
                        return new EnumValue(value, name, desc);
                    }
                default:
                    throw new JsonException($"无法将值解析成 EnumValue：必须是一个字符串或对象且对象应包含 value 属性\n{v}");
            }
        }

        private static ArrayType _readArrayType(JsonNode obj, Types types)
        {
            var elementObject = obj["elementType"];
            var elementType = ReadType(elementObject, types) ?? StringType.Default;
            return new ArrayType(elementType);
        }
    }
}