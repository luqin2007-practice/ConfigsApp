using System.Text.Json.Nodes;
using System.Windows;
using System.Windows.Controls;
using Configs.util;

namespace Configs.property;

/// <summary>
/// 表示一条属性，该属性将显示在一个 Grid 面板中
/// </summary>
/// <param name="propertyName">属性名</param>
/// <param name="title">属性显示名</param>
/// <param name="description">属性描述</param>
public abstract class Property(string propertyName, string title, string? description = null)
{
    /// <summary>
    /// Grid 面板中表示标题的列索引
    /// </summary>
    public const int ColumnTitle = 0;

    /// <summary>
    /// Grid 面板中表示属性内容的列索引
    /// </summary>
    public const int ColumnValue = 1;

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
    public string? Description => description;

    /// <summary>
    /// 应用程序当前属性值，可能与显示的不同
    /// 若获取失败则应显示异常信息
    /// </summary>
    public abstract object Value { get; }

    /// <summary>
    /// 获取当前显示的值
    /// </summary>
    public abstract object? InputValue { get; }

    protected FrameworkElement NameElement = new Label();
    protected FrameworkElement ValueElement = null!;

    /// <summary>
    /// 该属性是否可见
    /// </summary>
    public bool IsVisible
    {
        get => _isVisible;
        set
        {
            var v = value ? Visibility.Visible : Visibility.Hidden;
            NameElement.Visibility = v;
            ValueElement.Visibility = v;
            _isVisible = value;
        }
    }

    private bool _isVisible;

    public abstract FrameworkElement CreatePropertyElement();

    /// <summary>
    /// 在 Grid 中初始化视图
    /// </summary>
    /// <param name="panel">视图容器</param>
    /// <param name="row">属性所在行</param>
    /// <see cref="ColumnTitle"/>
    /// <see cref="ColumnValue"/>
    public void InitializeDisplay(Grid panel, int row)
    {
        // 初始化属性名
        if (NameElement is Label nameLabel)
        {
            nameLabel.Content = Title;
            if (Description != null)
            {
                nameLabel.ToolTip = PropertyName + '\n' + Description;
            }
            else
            {
                nameLabel.ToolTip = PropertyName;
            }
        }
        // 初始化属性内容
        ValueElement = CreatePropertyElement();
        // 初始化视图
        panel.Children.Add(NameElement);
        panel.Children.Add(ValueElement);
        Grid.SetColumn(NameElement, ColumnTitle);
        Grid.SetRow(NameElement, row);
        Grid.SetColumn(ValueElement, ColumnValue);
        Grid.SetRow(ValueElement, row);
    }

    /// <summary>
    /// 显示错误信息
    /// </summary>
    /// <param name="error">错误信息，值为空时表示清除异常信息</param>
    /// <param name="detail">详细信息，可能为空</param>
    public abstract void DisplayError(string? error, string? detail);

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