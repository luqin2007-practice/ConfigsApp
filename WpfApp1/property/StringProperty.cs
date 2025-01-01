using System.Text.Json.Nodes;
using System.Windows;
using Configs.app;
using Configs.widget.property;

namespace Configs.property;

/// <summary>
/// 单行字符串类型属性
/// </summary>
/// <param name="propertyName">属性名</param>
/// <param name="title">属性显示名</param>
/// <param name="description">属性描述</param>
public abstract class StringProperty(string propertyName, string title, string description, IType type, string defaultValue) 
    : Property(propertyName, title, description, type, defaultValue)
{
    private readonly Lazy<StringPropertyElement> _property = new(() => new StringPropertyElement());
    protected StringPropertyElement PropertyElement => _property.Value;

    public override object InputValue => PropertyElement.PropertyValue;

    public string ValueAsStr => (string)Value;

    public string InputValueAsStr => (string)InputValue;

    public string DefaultValueAsStr => (string)DefaultValue;

    public override FrameworkElement CreatePropertyElement()
    {
        PropertyElement.Initialize(this, ValueAsStr, defaultValue);
        return PropertyElement;
    }

    public override void Export(JsonObject values)
    {
        values[PropertyName] = InputValueAsStr;
    }

    public override void Import(JsonObject data)
    {
        if (data.TryGetPropertyValue(PropertyName, out var jv))
        {
            PropertyElement.PropertyValue = jv!.GetValue<string>();
        }
    }

    protected void ApplyInputValue()
    {
        PropertyElement.Error = null;
        var r = ApplyValue(InputValueAsStr);
        if (!r.IsOk)
        {
            PropertyElement.Error = ("属性设置失败", r.ErrorMessage);
        }
    }
}