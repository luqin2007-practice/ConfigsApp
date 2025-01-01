using Configs.app;
using Configs.util;

namespace Configs.property;

public class EnumFileProperty(ConfigFile file, ConfigProperty property) : EnumProperty(
    property.Property, property.Name, property.Desc, (EnumType)property.Type, property.Default.GetValue<EnumValue>())
{
    public override object Value => ValueType.StringToValue(file.GetProperty<string>(property));

    public override ExeResult ApplyValue(object value)
    {
        file.SetProperty(property, ValueType.ValueToString(value));
        return file.Save();
    }

    public override void Revoke()
    {
        file.RemoveProperty(property);
        PropertyElement.Error = file.Save().ToError("重置属性");
    }
}