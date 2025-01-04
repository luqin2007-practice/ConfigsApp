using Configs.app;
using Configs.property;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using static System.Windows.DependencyProperty;

namespace Configs.widget.property
{
    public class PropertyUserControl<T> : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty PropertyNameProperty = Register(nameof(PropertyName), typeof(string), typeof(PropertyUserControl<T>), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty PropertyKeyProperty = Register(nameof(PropertyKey), typeof(string), typeof(PropertyUserControl<T>), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty PropertyValueProperty = Register(nameof(PropertyValue), typeof(T), typeof(PropertyUserControl<T>), new PropertyMetadata(default(T)));
        public static readonly DependencyProperty DescriptionProperty = Register(nameof(Description), typeof(string), typeof(PropertyUserControl<T>), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty DefaultProperty = Register(nameof(Default), typeof(T), typeof(PropertyUserControl<T>), new PropertyMetadata(default(T)));
        public static readonly DependencyProperty ValueTypeProperty = Register(nameof(ValueType), typeof(IType), typeof(PropertyUserControl<T>), new PropertyMetadata(StringType.Default));
        public static readonly DependencyProperty ErrorProperty = Register(nameof(Error), typeof(Error), typeof(PropertyUserControl<T>), new PropertyMetadata(null));

        static PropertyUserControl()
        {
        }

        public string PropertyName
        {
            get => (string)GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }

        public string PropertyKey
        {
            get => (string)GetValue(PropertyKeyProperty);
            set => SetValue(PropertyKeyProperty, value);
        }

        public T PropertyValue
        {
            get => (T)GetValue(PropertyValueProperty);
            set => SetValue(PropertyValueProperty, value);
        }

        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public T Default
        {
            get => (T)GetValue(DefaultProperty);
            set => SetValue(DefaultProperty, value);
        }

        public IType ValueType
        {
            get => (IType)GetValue(ValueTypeProperty);
            set => SetValue(ValueTypeProperty, value);
        }

        public Error? Error
        {
            get => (Error?)GetValue(ErrorProperty);
            set => SetValue(ErrorProperty, value);
        }

        public string ValueStr => ValueType.ValueToString(PropertyValue!);

        public event PropertyChangedEventHandler? PropertyChanged;

        protected Property Property;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Initialize(Property property, T value, T defaultValue)
        {
            Property = property;

            PropertyName = property.PropertyName;
            PropertyKey = property.Title;
            Description = property.Description;
            ValueType = property.ValueType;
            PropertyValue = value;
            Default = defaultValue;
        }
    }

    public class StringPropertyUserControl : PropertyUserControl<string>
    {
    }

    public class BoolPropertyUserControl : PropertyUserControl<bool>
    {
    }

    public class EnumPropertyUserControl : PropertyUserControl<EnumValue>
    {
    }

    public class IntPropertyUserControl : PropertyUserControl<long>
    {
    }
}