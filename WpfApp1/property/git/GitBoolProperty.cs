using System.Text.Json.Nodes;
using Configs.app;
using Configs.util;

namespace Configs.property.git;

public class GitBoolProperty(GitAppContext context, string propertyName, string title, string description, bool defaultValue)
    : BoolProperty(propertyName, title, description)
{
    public override bool Value
    {
        get
        {
            var r = Context.GetGlobalProperty(PropertyName);
            if (r.IsOk)
            {
                return r.OutputMessage.Equals("true", StringComparison.OrdinalIgnoreCase);
            }
            
            DisplayError("获取属性失败", r.ErrorMessage);
            return defaultValue;
        }
    }

    public override ExeResult ApplyValue(bool value)
    {
        return Context.SetGlobalProperty(PropertyName, value ? "true" : "false");
    }

    protected override string GetValueString(bool value)
    {
        return value.ToString();
    }

    protected override bool TryParseJson(JsonNode? value, out bool result)
    {
        var v = value?.GetValue<bool>();
        result = v ?? defaultValue;
        return v != null;
    }

    public override void Revoke()
    {
        Context.UnsetGlobalProperty(PropertyName, string.Empty);
    }
}