using Configs.app;
using Configs.util;

namespace Configs.property;

public class StringFileProperty(ConfigFile file, ConfigProperty property) : StringProperty(
    property.Property, property.Name, property.Desc, property.Type, property.Default.GetValue<string>())
{
    public override object Value => file.GetProperty<string>(property);
    public override ExeResult ApplyValue(object value)
    {
        file.SetProperty(property, ValueAsStr);
        return file.Save();
    }

    public override void Revoke()
    {
        file.RemoveProperty(property);
        PropertyElement.Error = file.Save().ToError("重置属性");
    }
}