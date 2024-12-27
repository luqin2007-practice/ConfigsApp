using System.Text.Json.Nodes;
using System.Windows;
using System.Windows.Controls;
using Configs.widget;

namespace Configs.property;

/// <summary>
/// 布尔值类型属性
/// </summary>
/// <param name="propertyName">属性名</param>
/// <param name="title">属性显示名</param>
/// <param name="description">属性描述</param>
public abstract class BoolProperty(string propertyName, string title, string description)
    : Property(propertyName, title, description)
{
    private bool _skipInputEvent;
    private CheckBox? _value;
    private ErrorLabel? _error;

    public override object? InputValue => (bool)_value!.IsChecked!;

    /// <summary>
    /// 转换为布尔类型的值
    /// </summary>
    public bool ValueAsBool => (bool)Value;

    /// <summary>
    /// 转换为布尔类型的输入值
    /// </summary>
    public bool InputValueAsBool => (bool)InputValue;

    public override FrameworkElement CreatePropertyElement()
    {
        _error = new ErrorLabel();
        _value = new CheckBox
        {
            VerticalAlignment = VerticalAlignment.Center,
            IsChecked = ValueAsBool,
        };
        _value.Checked += (_, _) =>
        {
            // 当属性设置失败时，重置显示值，此时不触发事件
            if (_skipInputEvent) return;

            // 应用属性值
            DisplayError(null, null);
            var r = ApplyValue(_value.IsChecked ?? false);
            if (!r.IsOk)
            {
                _skipInputEvent = true;
                DisplayError("属性设置失败", r.ErrorMessage);
                _value.IsChecked = ValueAsBool;
                _skipInputEvent = false;
            }
            _value.Content = GetValueString(ValueAsBool);
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
        values[PropertyName] = InputValueAsBool;
    }

    public override void Import(JsonObject data)
    {
        if (data.TryGetPropertyValue(PropertyName, out var jv) && TryParseJson(jv, out var r))
        {
            _value!.IsChecked = r;
        }
    }

    /// <summary>
    ///     获取属性值的字符串表示，用于显示在 CheckBox 后
    /// </summary>
    /// <param name="value">属性值</param>
    /// <returns>CheckBox 后显示的值</returns>
    protected abstract string GetValueString(bool value);

    /// <summary>
    ///     尝试从 Json 中解析属性布尔值
    /// </summary>
    /// <param name="value">Json 值</param>
    /// <param name="result">解析出的布尔属性值</param>
    /// <returns>是否解析成功</returns>
    protected abstract bool TryParseJson(JsonNode? value, out bool result);
}