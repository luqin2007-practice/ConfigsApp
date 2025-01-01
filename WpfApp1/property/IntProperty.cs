using System.Text.Json.Nodes;
using System.Windows;
using Configs.app;
using Configs.widget.property;

namespace Configs.property;

public abstract class IntProperty(string propertyName, string title, string description, IType valueType, long defaultValue) : Property(propertyName, title, description, valueType, defaultValue)
{
    public override object InputValue => PropertyElement.PropertyValue;

    public long ValueAsNum => (long) Value;

    public long InputValueAsNum => (long) InputValue;

    public long DefaultValueAsNum => defaultValue;

    protected IntPropertyElement PropertyElement = new();

    public override FrameworkElement CreatePropertyElement()
    {
        PropertyElement.Initialize(this, ValueAsNum, defaultValue);
        return PropertyElement;
    }

    public override void Export(JsonObject values)
    {
        values[PropertyName] = InputValueAsNum;
    }

    public override void Import(JsonObject data)
    {
        if (data.TryGetPropertyValue(PropertyName, out var jv))
        {
            PropertyElement.PropertyValue = jv!.GetValue<long>();
        }
    }
}