﻿using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

namespace Configs.app;

/// <summary>
///     类型
/// </summary>
public interface IType
{
    /// <summary>
    ///     类型默认值
    /// </summary>
    public string DefaultValue { get; }

    /// <summary>
    ///     类型名
    /// </summary>
    public string Type { get; }

    /// <summary>
    ///     检查给定字符串是否符合该类型
    /// </summary>
    /// <param name="value">字符串</param>
    /// <returns>是否符合对应类型</returns>
    public bool CheckValue(string value);

    /// <summary>
    ///     将对应属性值转换为字符串类型表达
    /// </summary>
    /// <param name="value">属性值</param>
    /// <returns>字符串</returns>
    public string ValueToString(object value);

    /// <summary>
    ///     将字符串转换为对应值
    /// </summary>
    /// <param name="value">字符串值</param>
    /// <returns>属性值</returns>
    public object StringToValue(string value);
}

/// <summary>
///     字符串类型
/// </summary>
public class StringType : IType
{
    public static readonly StringType Default = new();

    private readonly Regex? _r;

    /// <summary>
    ///     字符串类型
    /// </summary>
    /// <param name="regex">属性测试正则表达式，默认不做验证</param>
    public StringType(string? regex = null)
    {
        Regex = regex;
        _r = regex == null ? null : new Regex(regex);
    }

    /// <summary>属性测试正则表达式，默认不做验证</summary>
    public string? Regex { get; init; }

    public string DefaultValue => "";

    public string Type => "string";

    public bool CheckValue(string value)
    {
        return _r?.IsMatch(value) ?? true;
    }

    public string ValueToString(object value)
    {
        return (string)value;
    }

    public object StringToValue(string value)
    {
        return value;
    }
}

/// <summary>
///     布尔类型
/// </summary>
public class BoolType : IType
{
    public static readonly BoolType Default = new();

    /// <summary>
    ///     布尔类型
    /// </summary>
    /// <param name="true">当值为 true 时对应的字符串</param>
    /// <param name="false">当值为 false 时对应的字符串</param>
    public BoolType(string @true = "true", string @false = "false")
    {
        True = @true;
        False = @false;
    }

    /// <summary>当值为 true 时对应的字符串</summary>
    public string True { get; init; }

    /// <summary>当值为 false 时对应的字符串</summary>
    public string False { get; init; }

    public string DefaultValue => False;

    public string Type => "bool";

    public bool CheckValue(string v)
    {
        return v == True || v == False;
    }

    public string ValueToString(object value)
    {
        return (bool)value ? True : False;
    }

    public object StringToValue(string value)
    {
        return value == True;
    }
}

/// <summary>
///     整数类型
/// </summary>
public class IntType : IType
{
    public static readonly IntType Default = new();

    /// <summary>
    ///     整数类型
    /// </summary>
    /// <param name="min">最小值，默认不做限制</param>
    /// <param name="max">最大值，默认不做限制</param>
    public IntType(long? min = null, long? max = null)
    {
        Min = min;
        Max = max;
    }

    /// <summary>最小值，默认不做限制</summary>
    public long? Min { get; init; }

    /// <summary>最大值，默认不做限制</summary>
    public long? Max { get; init; }

    public string DefaultValue => (Min ?? 0L).ToString();

    public string Type => "int";

    public bool CheckValue(string value)
    {
        return long.TryParse(value, out var i) && i >= (Min ?? i) && i <= (Max ?? i);
    }

    public string ValueToString(object value)
    {
        return value.ToString()!;
    }

    public object StringToValue(string value)
    {
        return long.Parse(value);
    }
}

/// <summary>
///     文件夹类型
/// </summary>
public class DirectoryType : IType
{
    public static readonly DirectoryType Default = new(false);
    public static readonly DirectoryType MustExist = new(true);

    /// <summary>
    ///     文件夹类型
    /// </summary>
    /// <param name="exist">文件夹是否必须存在</param>
    public DirectoryType(bool exist)
    {
        Exist = exist;
    }

    /// <summary>文件夹是否必须存在</summary>
    public bool Exist { get; init; }

    public string DefaultValue => "/";

    public string Type => "directory";

    public bool CheckValue(string value)
    {
        return Exist && Directory.Exists(value);
    }

    public string ValueToString(object value)
    {
        return (string)value;
    }

    public object StringToValue(string value)
    {
        return value;
    }
}

/// <summary>
///     文件类型
/// </summary>
public class FileType : IType
{
    public static readonly FileType Default = new();

    /// <summary>
    ///     文件类型
    /// </summary>
    /// <param name="exist">文件是否必须存在</param>
    /// <param name="ext">可接受的文件类型</param>
    public FileType(bool exist = false, List<string>? ext = null)
    {
        Exist = exist;
        Ext = ext;
    }

    /// <summary>文件是否必须存在</summary>
    public bool Exist { get; init; }

    /// <summary>可接受的文件类型</summary>
    public List<string>? Ext { get; init; }

    public string DefaultValue => "/";

    public string Type => "file";

    public bool CheckValue(string value)
    {
        if (Exist && !File.Exists(value)) return false;

        return Ext == null || Ext.Contains(Path.GetExtension(value));
    }

    public string ValueToString(object value)
    {
        return (string)value;
    }

    public object StringToValue(string value)
    {
        return value;
    }
}

/// <summary>
///     列表
/// </summary>
public class ListType : IType
{
    public static readonly ListType Default = new("\n", StringType.Default);

    /// <summary>
    ///     列表
    /// </summary>
    /// <param name="split">列表分隔符</param>
    /// <param name="elementType">每项数据的类型</param>
    public ListType(string split, IType elementType)
    {
        Split = split;
        ElementType = elementType;
    }

    /// <summary>列表分隔符</summary>
    public string Split { get; init; }

    /// <summary>每项数据的类型</summary>
    public IType ElementType { get; init; }

    public string DefaultValue => "";

    public string Type => "list";

    public bool CheckValue(string value)
    {
        return value
            .Split(Split)
            .Select(s => s.Trim())
            .Select(s => ElementType.CheckValue(s))
            .All(s => s);
    }

    public string ValueToString(object value)
    {
        return string.Join(Split,
            from object? o in (IEnumerable)value select (ElementType ?? StringType.Default).ValueToString(o));
    }

    public object StringToValue(string value)
    {
        return value.Split(Split)
            .Select(s => ElementType.StringToValue(s))
            .ToList();
    }
}

/// <summary>
///     枚举类型
/// </summary>
public class EnumType : IType
{
    private readonly Dictionary<string, EnumValue> _dictionary;
    private readonly HashSet<string> _values;

    /// <summary>
    ///     枚举类型
    /// </summary>
    /// <param name="type">枚举名</param>
    /// <param name="values">所有枚举</param>
    public EnumType(string type, List<EnumValue> values)
    {
        Type = type;
        Values = values;
        _values = values.Select(s => s.Value).ToHashSet();
        _dictionary = values.ToDictionary(s => s.Value);
    }

    /// <summary>所有枚举</summary>
    public List<EnumValue> Values { get; init; }

    public string DefaultValue => Values[0].Value;

    /// <summary>枚举名</summary>
    public string Type { get; init; }

    public bool CheckValue(string value)
    {
        return _values.Contains(value);
    }

    public string ValueToString(object value)
    {
        return ((EnumValue)value).Value;
    }

    public object StringToValue(string value)
    {
        return _dictionary.TryGetValue(value, out var v) ? v : Values[0];
    }
}

public class EnumValue(string value, string name, string desc)
{
    public string Value { get; init; } = value;
    public string Name { get; init; } = name;
    public string Desc { get; init; } = desc;
}

public class ArrayType(IType elementType) : IType
{
    public static readonly ArrayType Default = new(StringType.Default);

    public IType ElementType { get; init; } = elementType;

    public string DefaultValue => "";
    public string Type => "array";

    public bool CheckValue(string value)
    {
        return true;
    }

    public string ValueToString(object value)
    {
        throw new NotSupportedException("array 类型不支持直接的类型转换，请根据文档类型自行处理");
    }

    public object StringToValue(string value)
    {
        throw new NotSupportedException("array 类型不支持直接的类型转换，请根据文档类型自行处理");
    }
}