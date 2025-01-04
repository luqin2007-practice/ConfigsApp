using Configs.app;
using Configs.widget.property;
using System.Text.Json.Nodes;
using System.Windows;

namespace Configs.property
{
    /// <summary>
    /// 单行字符串类型属性
    /// </summary>
    public abstract class StringProperty : Property
    {
        private readonly Lazy<StringPropertyElement> _property = new Lazy<StringPropertyElement>(() => new StringPropertyElement());
        private readonly string _defaultValue;

        /// <summary>
        /// 单行字符串类型属性
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="title">属性显示名</param>
        /// <param name="description">属性描述</param>
        /// <param name="type">属性类型</param>
        /// <param name="defaultValue">默认值</param>
        protected StringProperty(string propertyName, string title, string description, IType type, string defaultValue)
            : base(propertyName, title, description, type, defaultValue)
        {
            _defaultValue = defaultValue;
        }

        protected StringPropertyElement PropertyElement => _property.Value;

        public override object InputValue => PropertyElement.PropertyValue;

        public string ValueAsStr => (string)Value;

        public string InputValueAsStr => (string)InputValue;

        public string DefaultValueAsStr => (string)DefaultValue;

        public override FrameworkElement CreatePropertyElement()
        {
            PropertyElement.Initialize(this, ValueAsStr, _defaultValue);
            return PropertyElement;
        }

        public override void ReloadValue()
        {
            PropertyElement.PropertyValue = ValueAsStr;
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
    }
}