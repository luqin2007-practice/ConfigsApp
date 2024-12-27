using System.Text.Json.Nodes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Configs.widget;

namespace Configs.property;

/// <summary>
/// 单行字符串类型属性
/// </summary>
/// <param name="propertyName">属性名</param>
/// <param name="title">属性显示名</param>
/// <param name="description">属性描述</param>
public abstract class StringProperty(string propertyName, string title, string description) 
    : Property(propertyName, title, description)
{
    private ErrorLabel? _error;
    private TextBox? _value;

    public override object? InputValue => _value!.Text;

    public string ValueAsStr => (string)Value;

    public string InputValueAsStr => (string)InputValue;

    public override FrameworkElement CreatePropertyElement()
    {
        _error = new ErrorLabel();
        _value = new TextBox()
        {
            Text = ValueAsStr,
            VerticalAlignment = VerticalAlignment.Center
        };
        _value.LostFocus += (_, _) => ApplyInputValue();
        _value.KeyDown += (_, e) =>
        {
            if (e.IsDown && e.Key == Key.Enter)
            {
                ApplyInputValue();
            }
        };

        var stack = new StackPanel();
        stack.Children.Add(_value);
        stack.Children.Add(_error);
        return stack;
    }

    public override void DisplayError(string? error, string? detail)
    {
        if (error == null)
        {
            _error?.Hide();
        }
        else
        {
            _error?.Show(error, detail);
        }
    }

    public override void Export(JsonObject values)
    {
        values[PropertyName] = InputValueAsStr;
    }

    public override void Import(JsonObject data)
    {
        if (data.TryGetPropertyValue(PropertyName, out var jv))
        {
            _value!.Text = jv!.GetValue<string>();
        }
    }

    protected void ApplyInputValue()
    {
        // 应用属性值
        DisplayError(null, null);
        var r = ApplyValue(_value!.Text);
        if (!r.IsOk)
        {
            // 由于字符串值可能会连续编辑，不需要重置
            DisplayError("属性设置失败", r.ErrorMessage);
        }
    }
}