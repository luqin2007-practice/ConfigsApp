using Configs.app;
using Configs.widget.property;
using System.Text.Json.Nodes;
using System.Windows;

namespace Configs.property
{
    /// <summary>
    /// 布尔值类型属性
    /// </summary>
    public abstract class BoolProperty : Property
    {
        private readonly Lazy<BoolPropertyElement> _element = new Lazy<BoolPropertyElement>(() => new BoolPropertyElement());
        private readonly bool _defaultValue;

        /// <summary>
        /// 布尔值类型属性
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="title">属性显示名</param>
        /// <param name="description">属性描述</param>
        /// <param name="type">属性类型</param>
        /// <param name="defaultValue">默认值</param>
        protected BoolProperty(string propertyName, string title, string description, IType type, bool defaultValue) : base(propertyName, title, description, type, defaultValue)
        {
            _defaultValue = defaultValue;
        }

        protected BoolPropertyElement PropertyElement => _element.Value;

        public override object? InputValue => PropertyElement.PropertyValue;

        /// <summary>
        /// 转换为布尔类型的值
        /// </summary>
        public bool ValueAsBool => (bool)Value;

        /// <summary>
        /// 转换为布尔类型的输入值
        /// </summary>
        public bool InputValueAsBool => (bool)InputValue;

        /// <summary>
        /// 转换为布尔值的默认值
        /// </summary>
        public bool DefaultValueAsBool => _defaultValue;

        public override void ReloadValue()
        {
            PropertyElement.PropertyValue = ValueAsBool;
        }

        public override FrameworkElement CreatePropertyElement()
        {
            PropertyElement.Initialize(this, ValueAsBool, _defaultValue);
            return PropertyElement;
        }

        public override void Export(JsonObject values)
        {
            values[PropertyName] = InputValueAsBool;
        }

        public override void Import(JsonObject data)
        {
            if (data.TryGetPropertyValue(PropertyName, out var jv) && TryParseJson(jv, out var r))
            {
                PropertyElement.PropertyValue = r;
            }
        }

        /// <summary>
        /// 尝试从 Json 中解析属性布尔值
        /// </summary>
        /// <param name="value">Json 值</param>
        /// <param name="result">解析出的布尔属性值</param>
        /// <returns>是否解析成功</returns>
        protected abstract bool TryParseJson(JsonNode? value, out bool result);
    }
}