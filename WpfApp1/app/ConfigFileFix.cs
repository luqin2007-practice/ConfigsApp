using System.Text.Json;
using System.Text.Json.Nodes;
using Configs.util.jsonConverter;
using TypeConverter = Configs.util.jsonConverter.TypeConverter;

namespace Configs.app;

public partial class ConfigFile
{
    public void Fix(string name, JsonObject desc, JsonObject? group, Types types)
    {
        Name = name;
        foreach (var (key, value) in desc)
        {
            JsonNode? g = null;
            group?.TryGetPropertyValue(key, out g);
            _addGroupOrProperty(key, value!.AsObject(), g, Root, types);
        }
    }

    private void _addGroupOrProperty(string property, JsonObject desc, JsonNode? group, ConfigGroup parent, Types types)
    {
        if (_isProperty(desc, group))
            _addProperty(property, desc, parent, types);
        else
            _addGroup(property, desc, group, parent, types);
    }

    private bool _isProperty(JsonObject desc, JsonNode? group)
    {
        // group 组有记录，一定不是属性
        if (group != null) return false;

        // 属性除 type 和 default 外，都是值而非对象
        foreach (var (key, node) in desc)
        {
            if (key is "type" or "default") continue;

            if (node is JsonObject) return false;
        }

        return true;
    }

    private void _addProperty(string property, JsonObject desc, ConfigGroup parent, Types types)
    {
        // 属性显示名
        var name = desc["name"]?.GetValue<string>() ?? property;
        // 属性描述
        var descStr = desc["desc"]?.GetValue<string>() ?? name;
        // 属性类型
        desc.TryGetPropertyValue("type", out var n);
        var type = TypeConverter.ReadType(n, types) ?? StringType.Default;
        // 属性默认值
        var defaultValue = DefaultValueConverter.ReadFromJson(desc["default"]) ?? new DefaultValue();
        defaultValue.Fix(type);
        // 是否隐藏
        var hidden = desc["hidden"]?.GetValue<bool>() ?? false;

        parent.Properties.Add(new ConfigProperty(property, name, descStr, type, defaultValue, hidden, parent));
    }

    private void _addGroup(string groupName, JsonObject desc, JsonNode? group, ConfigGroup parent, Types types)
    {
        ConfigGroup g;
        JsonObject? children = null;
        switch (group)
        {
            // 没有 group 数据，使用默认值
            case null:
                g = new ConfigGroup(groupName, groupName, groupName, [], [], parent);
                break;

            // group 数据为字符串，使用字符串作为组名
            case JsonValue gv:
            {
                var gName = gv.GetValue<string>();
                g = new ConfigGroup(groupName, gName, groupName, [], [], parent);
                break;
            }

            // group 数据为对象
            case JsonObject:
            {
                // group 显示名
                var gName = group["name"]?.GetValue<string>() ?? groupName;
                // group 描述
                var gDesc = group["desc"]?.GetValue<string>() ?? gName;

                g = new ConfigGroup(groupName, gName, gDesc, [], [], parent);
                children = group["children"]?.AsObject();
                break;
            }

            default:
                throw new JsonException($"未知 group 描述类型：{group}");
        }

        parent.Groups.Add(g);
        foreach (var (cName, cNode) in desc) _addGroupOrProperty(cName, cNode!.AsObject(), children?[cName], g, types);
    }
}