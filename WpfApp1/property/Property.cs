using System.Text.Json.Nodes;
using System.Windows;
using System.Windows.Controls;
using Configs.util;

namespace Configs.property;

/// <summary>
/// ��ʾһ�����ԣ������Խ���ʾ��һ�� Grid �����
/// </summary>
/// <param name="propertyName">������</param>
/// <param name="title">������ʾ��</param>
/// <param name="description">��������</param>
public abstract class Property(string propertyName, string title, string? description = null)
{
    /// <summary>
    /// Grid ����б�ʾ�����������
    /// </summary>
    public const int ColumnTitle = 0;

    /// <summary>
    /// Grid ����б�ʾ�������ݵ�������
    /// </summary>
    public const int ColumnValue = 1;

    /// <summary>
    /// ������
    /// </summary>
    public string PropertyName => propertyName;

    /// <summary>
    /// ������ʾ��
    /// </summary>
    public string Title => title;

    /// <summary>
    /// ��������
    /// </summary>
    public string? Description => description;

    /// <summary>
    /// Ӧ�ó���ǰ����ֵ����������ʾ�Ĳ�ͬ
    /// ����ȡʧ����Ӧ��ʾ�쳣��Ϣ
    /// </summary>
    public abstract object Value { get; }

    /// <summary>
    /// ��ȡ��ǰ��ʾ��ֵ
    /// </summary>
    public abstract object? InputValue { get; }

    protected FrameworkElement NameElement = new Label();
    protected FrameworkElement ValueElement = null!;

    /// <summary>
    /// �������Ƿ�ɼ�
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
    /// �� Grid �г�ʼ����ͼ
    /// </summary>
    /// <param name="panel">��ͼ����</param>
    /// <param name="row">����������</param>
    /// <see cref="ColumnTitle"/>
    /// <see cref="ColumnValue"/>
    public void InitializeDisplay(Grid panel, int row)
    {
        // ��ʼ��������
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
        // ��ʼ����������
        ValueElement = CreatePropertyElement();
        // ��ʼ����ͼ
        panel.Children.Add(NameElement);
        panel.Children.Add(ValueElement);
        Grid.SetColumn(NameElement, ColumnTitle);
        Grid.SetRow(NameElement, row);
        Grid.SetColumn(ValueElement, ColumnValue);
        Grid.SetRow(ValueElement, row);
    }

    /// <summary>
    /// ��ʾ������Ϣ
    /// </summary>
    /// <param name="error">������Ϣ��ֵΪ��ʱ��ʾ����쳣��Ϣ</param>
    /// <param name="detail">��ϸ��Ϣ������Ϊ��</param>
    public abstract void DisplayError(string? error, string? detail);

    /// <summary>
    /// Ӧ������ֵ�����������������ʾ
    /// </summary>
    /// <param name="value">������ֵ</param>
    /// <returns>���ý��</returns>
    public abstract ExeResult ApplyValue(object value);

    /// <summary>
    /// �������ԣ������Դ��� Json ������
    /// </summary>
    /// <param name="values">���Զ���</param>
    public abstract void Export(JsonObject values);

    /// <summary>
    /// �������Դ� Json �����ж�ȡ����
    /// </summary>
    /// <param name="data">���Զ���</param>
    public abstract void Import(JsonObject data);

    /// <summary>
    /// ȡ��������ָ�Ĭ��ֵ
    /// </summary>
    public abstract void Revoke();
}