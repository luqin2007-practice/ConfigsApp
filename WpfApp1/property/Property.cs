using Configs.app;
using Configs.util;
using System.Text.Json.Nodes;
using System.Windows;
using System.Windows.Controls;

namespace Configs.property
{
    /// <summary>
    /// ��ʾһ�����ԣ������Խ���ʾ��һ�� Grid �����
    /// </summary>
    public abstract class Property
    {
        /// <summary>
        /// ������
        /// </summary>
        public string PropertyName => _propertyName;

        /// <summary>
        /// ������ʾ��
        /// </summary>
        public string Title => _title;

        /// <summary>
        /// ��������
        /// </summary>
        public string Description => _description;

        /// <summary>
        /// Ӧ�ó���ǰ����ֵ����������ʾ�Ĳ�ͬ
        /// ����ȡʧ����Ӧ��ʾ�쳣��Ϣ
        /// </summary>
        public abstract object Value { get; }

        /// <summary>
        /// ��������
        /// </summary>
        public IType ValueType { get; }

        /// <summary>
        /// ��ȡ��ǰ��ʾ��ֵ
        /// </summary>
        public abstract object? InputValue { get; }

        /// <summary>
        /// ���Ե�Ĭ��ֵ
        /// </summary>
        public object DefaultValue;

        protected FrameworkElement Element;

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
        private readonly string _propertyName;
        private readonly string _title;
        private readonly string _description;

        /// <summary>
        /// ��ʾһ�����ԣ������Խ���ʾ��һ�� Grid �����
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="title">������ʾ��</param>
        /// <param name="description">��������</param>
        /// <param name="valueType">��������</param>
        /// <param name="defaultValue">����Ĭ��ֵ</param>
        protected Property(string propertyName, string title, string description, IType valueType, object defaultValue)
        {
            _propertyName = propertyName;
            _title = title;
            _description = description;
            ValueType = valueType;
            DefaultValue = defaultValue;
        }

        /// <summary>
        /// ��ʼ����ͼ
        /// </summary>
        public abstract FrameworkElement CreatePropertyElement();

        public abstract void ReloadValue();

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
}