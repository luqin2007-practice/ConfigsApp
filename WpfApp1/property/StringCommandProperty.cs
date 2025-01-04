using Configs.app;
using Configs.util;
using Configs.widget;

namespace Configs.property
{
    public class StringCommandProperty(string app, Command command) 
        : StringProperty(command.Property, command.Name, command.Desc, command.Type, command.Default.GetValue<string>())
    {
        public override object Value
        {
            get
            {
                var r = CommandUtils.Execute(command.Commands.Read, app);
                PropertyElement.Error = r.ToError("获取属性失败");
                return r.OutputMessage.Trim();
            }
        }

        public override ExeResult ApplyValue(object value)
        {
            return ValueType.CheckValue((string)value) 
                ? CommandUtils.Execute(command.Commands.Write, app, value) 
                : ExeResult.Error("属性校验不通过");
        }

        public override void Revoke()
        {
            PropertyElement.Error = CommandUtils.Execute(command.Commands.Revoke, app).ToError("重置失败");
        }
    }
}