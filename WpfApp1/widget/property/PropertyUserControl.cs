using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Configs.app;
using Configs.property;

namespace Configs.widget.property;

public class PropertyUserControl<T> : UserControl, INotifyPropertyChanged
{
    private static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register(
        nameof(PropertyName), typeof(string), typeof(PropertyUserControl<T>), new PropertyMetadata(default(string)));

    private static readonly DependencyProperty PropertyKeyProperty = DependencyProperty.Register(
        nameof(PropertyKey), typeof(string), typeof(PropertyUserControl<T>), new PropertyMetadata(default(string)));

    private static readonly DependencyProperty PropertyValueProperty = DependencyProperty.Register(
        nameof(PropertyValue), typeof(T), typeof(PropertyUserControl<T>), new PropertyMetadata(default(T)));

    private static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
        nameof(Description), typeof(string), typeof(PropertyUserControl<T>), new PropertyMetadata(default(string)));

    private static readonly DependencyProperty DefaultProperty = DependencyProperty.Register(
        nameof(Default), typeof(T), typeof(PropertyUserControl<T>), new PropertyMetadata(default(T)));

    private static readonly DependencyProperty ValueTypeProperty = DependencyProperty.Register(
        nameof(ValueType), typeof(IType), typeof(PropertyUserControl<T>), new PropertyMetadata(StringType.Default));

    private static readonly DependencyProperty ErrorProperty = DependencyProperty.Register(
        nameof(Error), typeof(Error), typeof(PropertyUserControl<T>), new PropertyMetadata(null));

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

    public string ValueStr => ValueType.ValueToString(PropertyValue ?? Default!);

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void Initialize(Property property, T value, T defaultValue)
    {
        PropertyName = property.PropertyName;
        PropertyKey = property.Title;
        Description = property.Description;
        ValueType = property.ValueType;
        PropertyValue = value;
        Default = defaultValue;
    }
}

public class StringPropertyUserControl : PropertyUserControl<string>;

public class BoolPropertyUserControl : PropertyUserControl<bool>;