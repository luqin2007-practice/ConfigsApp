using System.Text.Json.Nodes;
using System.Windows;
using System.Windows.Controls;
using Configs.app;
using Configs.util;

namespace Configs.property;

/// <summary>
/// 表示一条属性，该属性将显示在一个 Grid 面板中
/// </summary>
/// <param name="propertyName">属性名</param>
/// <param name="title">属性显示名</param>
/// <param name="description">属性描述</param>
/// <param name="valueType">属性类型</param>
/// <param name="defaultValue">属性默认值</param>
public abstract class Property(string propertyName, string title, string description, IType valueType, object defaultValue)
{
    /// <summary>
    /// 属性名
    /// </summary>
    public string PropertyName => propertyName;

    /// <summary>
    /// 属性显示名
    /// </summary>
    public string Title => title;

    /// <summary>
    /// 属性描述
    /// </summary>
    public string Description => description;

    /// <summary>
    /// 应用程序当前属性值，可能与显示的不同
    /// 若获取失败则应显示异常信息
    /// </summary>
    public abstract object Value { get; }

    /// <summary>
    /// 属性类型
    /// </summary>
    public IType ValueType { get; } = valueType;

    /// <summary>
    /// 获取当前显示的值
    /// </summary>
    public abstract object? InputValue { get; }

    /// <summary>
    /// 属性的默认值
    /// </summary>
    public object DefaultValue = defaultValue;

    protected FrameworkElement Element = null!;

    /// <summary>
    /// 该属性是否可见
    /// </summary>
    public bool IsVisible
    {
        get => _isVisible;
        set
        {
            var v = value ? Visibility.Visible : Visibility.Hidden;
            Element.Visibility = v;
            _isVisible = value;
        }
    }

    private bool _isVisible;

    /// <summary>
    /// 初始化视图
    /// </summary>
    public abstract FrameworkElement CreatePropertyElement();

    public void InitializeDisplay(StackPanel panel)
    {
        Element = CreatePropertyElement();
        panel.Children.Add(Element);
    }

    /// <summary>
    /// 应用属性值，但不会更新属性显示
    /// </summary>
    /// <param name="value">新属性值</param>
    /// <returns>设置结果</returns>
    public abstract ExeResult ApplyValue(object value);

    /// <summary>
    /// 导出属性，将属性存入 Json 对象中
    /// </summary>
    /// <param name="values">属性对象</param>
    public abstract void Export(JsonObject values);

    /// <summary>
    /// 导入属性从 Json 对象中读取属性
    /// </summary>
    /// <param name="data">属性对象</param>
    public abstract void Import(JsonObject data);

    /// <summary>
    /// 取消变更，恢复默认值
    /// </summary>
    public abstract void Revoke();
}