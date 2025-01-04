using Configs.app;
using Configs.util;
using System.Text.Json.Nodes;

namespace Configs.property
{
    public class BoolCommandProperty(string app, Command command)
        : BoolProperty(command.Property, command.Name, command.Desc, command.Type, command.Default.GetValue<bool>())
    {
        public override object Value
        {
            get
            {
                var r = CommandUtils.Execute(command.Commands.Read, app);
                PropertyElement.Error = r.ToError("获取属性失败");
                return r.IsOk
                    ? command.Type.StringToValue(r.OutputMessage.Trim())
                    : DefaultValueAsBool;
            }
        }

        public override ExeResult ApplyValue(object value)
        {
            return ValueType.CheckValue(ValueType.ValueToString(value)) 
                ? CommandUtils.Execute(command.Commands.Write, app, command.Type.ValueToString(value)) 
                : ExeResult.Error("属性校验不通过");
        }

        public override void Revoke()
        {
            PropertyElement.Error = CommandUtils.Execute(command.Commands.Revoke, app).ToError("重置失败");
        }

        protected override bool TryParseJson(JsonNode? value, out bool result)
        {
            var v = value?.GetValue<bool>();
            result = v ?? DefaultValueAsBool;
            return v != null;
        }
    }
}