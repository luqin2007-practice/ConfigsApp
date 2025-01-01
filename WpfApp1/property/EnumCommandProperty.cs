using Configs.app;
using Configs.util;

namespace Configs.property;

public class EnumCommandProperty(string app, Command command) : EnumProperty(
    command.Property, command.Name, command.Desc, (EnumType) command.Type, command.Default.GetValue<EnumValue>())
{
    public override object Value
    {
        get
        {
            var r = CommandUtils.Execute(command.Commands.Read, app);
            PropertyElement.Error = r.ToError("获取属性失败");
            return ValueType.StringToValue(r.OutputMessage.Trim());
        }
    }

    public override ExeResult ApplyValue(object value)
    {
        return CommandUtils.Execute(command.Commands.Write, app, ((EnumValue)value).Value);
    }

    public override void Revoke()
    {
        PropertyElement.Error = CommandUtils.Execute(command.Commands.Revoke, app).ToError("重置失败");
    }
}