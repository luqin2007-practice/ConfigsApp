using Configs.app;
using Configs.util;

namespace Configs.property.git;

public class GitStringProperty(GitAppContext context, string propertyName, string title, string description)
    : StringProperty(propertyName, title, description)
{
    public override string Value
    {
        get
        {
            var r = Context.GetGlobalProperty(PropertyName);
            if (!r.IsOk)
            {
                DisplayError("获取属性失败", r.ErrorMessage);
            }

            return r.OutputMessage;
        }
    }

    public override ExeResult ApplyValue(string value)
    {
        return Context.SetGlobalProperty(PropertyName, value);
    }

    public override void Revoke()
    {
        Context.UnsetGlobalProperty(PropertyName, string.Empty);
    }
}