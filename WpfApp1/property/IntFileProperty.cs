using Configs.app;
using Configs.util;

namespace Configs.property
{
    public class IntFileProperty(ConfigFile file, ConfigProperty property) : IntProperty(property.Property,
        property.Name, property.Desc, property.Type, property.Default.GetValue<long>())
    {
        public override object Value => file.GetProperty<long>(property);

        public override ExeResult ApplyValue(object value)
        {
            if (!ValueType.CheckValue(ValueType.ValueToString(value)))
            {
                return ExeResult.Error("属性校验不通过");
            }

            file.SetProperty(property, (long)value);
            return file.Save();
        }

        public override void Revoke()
        {
            file.RemoveProperty(property);
            PropertyElement.Error = file.Save().ToError("重置属性");
        }
    }
}