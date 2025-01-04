using Configs.app;
using Configs.widget.property;
using System.Text.Json.Nodes;
using System.Windows;

namespace Configs.property
{
    public abstract class EnumProperty(string propertyName, string title, string description, EnumType valueType, EnumValue defaultValue) 
        : Property(propertyName, title, description, valueType, defaultValue)
    {
        public string ValueAsString => ((EnumValue)Value).Value;

        public string InputValueAsString => ((EnumValue)InputValue).Value;

        public string DefaultValueAsString => defaultValue.Value;

        protected EnumPropertyElement PropertyElement = new EnumPropertyElement();

        public override object InputValue => PropertyElement.PropertyValue;

        public override void ReloadValue()
        {
            PropertyElement.PropertyValue = (EnumValue)Value;
        }

        public override FrameworkElement CreatePropertyElement()
        {
            PropertyElement.Initialize(this, (EnumValue)Value, defaultValue);
            return PropertyElement;
        }

        public override void Export(JsonObject values)
        {
            values[PropertyName] = ValueAsString;
        }

        public override void Import(JsonObject data)
        {
            if (data.TryGetPropertyValue(PropertyName, out var jv))
            {
                PropertyElement.PropertyValue = (EnumValue)ValueType.StringToValue(jv!.GetValue<string>());
            }
        }
    }
}