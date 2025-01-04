using Configs.app;
using Configs.util;
using System.Text.Json.Nodes;

namespace Configs.property
{
    public class BoolFileProperty(ConfigFile file, ConfigProperty property) : BoolProperty(property.Property,
        property.Name, property.Desc, property.Type, property.Default.GetValue<bool>())
    {
        public override object Value => file.GetProperty<bool>(property);

        public override ExeResult ApplyValue(object value)
        {
            if (!ValueType.CheckValue(ValueType.ValueToString(value)))
            {
                return ExeResult.Error("属性校验不通过");
            }

            file.SetProperty(property, value);
            return file.Save();
        }

        public override void Revoke()
        {
            file.RemoveProperty(property);
            PropertyElement.Error = file.Save().ToError("重置属性");
        }

        protected override bool TryParseJson(JsonNode? value, out bool result)
        {
            var v = value?.GetValue<bool>();
            result = v ?? DefaultValueAsBool;
            return v != null;
        }
    }
}