using System.Text.Json.Nodes;
using System.Windows;
using Configs.app;
using Configs.widget.property;

namespace Configs.property;

public abstract class EnumProperty(string propertyName, string title, string description, EnumType valueType, EnumValue defaultValue) : Property(propertyName, title, description, valueType, defaultValue)
{
    public string ValueAsString => ((EnumValue)Value).Value;

    public string InputValueAsString => ((EnumValue)InputValue).Value;

    public string DefaultValueAsString => defaultValue.Value;

    protected EnumPropertyElement PropertyElement = new();

    public override object InputValue => PropertyElement.PropertyValue;

    public override FrameworkElement CreatePropertyElement()
    {
        PropertyElement.Initialize(this, (EnumValue) Value, defaultValue);
        return PropertyElement;
    }

    public override void Export(JsonObject values)
    {
        values[PropertyName] = ValueAsString;
    }

    public override void Import(JsonObject data)
    {
        if (data.TryGetPropertyValue(PropertyName, out var jv))
        {
            PropertyElement.PropertyValue = (EnumValue) ValueType.StringToValue(jv!.GetValue<string>());
        }
    }
}