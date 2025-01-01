using System.Text.Json.Nodes;
using System.Windows;
using System.Windows.Controls;
using Configs.app;
using Configs.util;

namespace Configs.property;

/// <summary>
/// ��ʾһ�����ԣ������Խ���ʾ��һ�� Grid �����
/// </summary>
/// <param name="propertyName">������</param>
/// <param name="title">������ʾ��</param>
/// <param name="description">��������</param>
/// <param name="valueType">��������</param>
/// <param name="defaultValue">����Ĭ��ֵ</param>
public abstract class Property(string propertyName, string title, string description, IType valueType, object defaultValue)
{
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
    public string Description => description;

    /// <summary>
    /// Ӧ�ó���ǰ����ֵ����������ʾ�Ĳ�ͬ
    /// ����ȡʧ����Ӧ��ʾ�쳣��Ϣ
    /// </summary>
    public abstract object Value { get; }

    /// <summary>
    /// ��������
    /// </summary>
    public IType ValueType { get; } = valueType;

    /// <summary>
    /// ��ȡ��ǰ��ʾ��ֵ
    /// </summary>
    public abstract object? InputValue { get; }

    /// <summary>
    /// ���Ե�Ĭ��ֵ
    /// </summary>
    public object DefaultValue = defaultValue;

    protected FrameworkElement Element = null!;

    /// <summary>
    /// �������Ƿ�ɼ�
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
    /// ��ʼ����ͼ
    /// </summary>
    public abstract FrameworkElement CreatePropertyElement();

    public void InitializeDisplay(StackPanel panel)
    {
        Element = CreatePropertyElement();
        panel.Children.Add(Element);
    }

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