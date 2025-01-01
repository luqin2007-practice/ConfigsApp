using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

namespace Configs.app;

/// <summary>
/// 类型
/// </summary>
public interface IType
{
    /// <summary>
    /// 类型默认值
    /// </summary>
    public string DefaultValue { get; }

    /// <summary>
    /// 类型名
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// 检查给定字符串是否符合该类型
    /// </summary>
    /// <param name="value">字符串</param>
    /// <returns>是否符合对应类型</returns>
    public bool CheckValue(string value);

    /// <summary>
    /// 将对应属性值转换为字符串类型表达
    /// </summary>
    /// <param name="value">属性值</param>
    /// <returns>字符串</returns>
    public string ValueToString(object value);
    
    /// <summary>
    /// 将字符串转换为对应值
    /// </summary>
    /// <param name="value">字符串值</param>
    /// <returns>属性值</returns>
    public object StringToValue(string value);
}

/// <summary>
/// 字符串类型
/// </summary>
/// <param name="Regex">属性测试正则表达式，默认不做验证</param>
public record StringType(string? Regex = null) : IType
{
    public static readonly StringType Default = new();

    private readonly Regex? _r = Regex == null ? null : new Regex(Regex);

    public string DefaultValue => "";

    public string Type => "string";

    public bool CheckValue(string value) => _r?.IsMatch(value) ?? true;

    public string ValueToString(object value) => (string)value;

    public object StringToValue(string value) => value;
}

/// <summary>
/// 布尔类型
/// </summary>
/// <param name="True">当值为 true 时对应的字符串</param>
/// <param name="False">当值为 false 时对应的字符串</param>
public record BoolType(string True = "true", string False = "false") : IType
{
    public static readonly BoolType Default = new BoolType();
    public string DefaultValue => False;

    public string Type => "bool";

    public bool CheckValue(string v) => v == True || v == False;

    public string ValueToString(object value) => (bool)value ? True : False;

    public object StringToValue(string value) => value == True;
}

/// <summary>
/// 整数类型
/// </summary>
/// <param name="Min">最小值，默认不做限制</param>
/// <param name="Max">最大值，默认不做限制</param>
public record IntType(long? Min = null, long? Max = null) : IType
{
    public static readonly IntType Default = new();
    public string DefaultValue => (Min ?? 0L).ToString();

    public string Type => "int";

    public bool CheckValue(string value) =>
        long.TryParse(value, out var i) && i >= (Min ?? i) && i <= (Max ?? i);

    public string ValueToString(object value) => value.ToString()!;

    public object StringToValue(string value) => long.Parse(value);
}

/// <summary>
/// 文件夹类型
/// </summary>
/// <param name="Exist">文件夹是否必须存在</param>
public record DirectoryType(bool Exist) : IType
{
    public static readonly DirectoryType Default = new DirectoryType(false);
    public static readonly DirectoryType MustExist = new DirectoryType(true);

    public string DefaultValue => "/";

    public string Type => "directory";

    public bool CheckValue(string value) => Exist && Directory.Exists(value);

    public string ValueToString(object value) => (string)value;

    public object StringToValue(string value) => value;
}

/// <summary>
/// 文件类型
/// </summary>
/// <param name="Exist">文件是否必须存在</param>
/// <param name="Ext">可接受的文件类型</param>
public record FileType(bool Exist = false, List<string>? Ext = null) : IType
{
    public static readonly FileType Default = new FileType();

    public string DefaultValue => "/";

    public string Type => "file";

    public bool CheckValue(string value)
    {
        if (Exist && !File.Exists(value))
        {
            return false;
        }

        return Ext == null || Ext.Contains(Path.GetExtension(value));
    }

    public string ValueToString(object value) => (string)value;

    public object StringToValue(string value) => value;
}

/// <summary>
/// 列表
/// </summary>
/// <param name="Split">列表分隔符</param>
/// <param name="ElementType">每项数据的类型</param>
public record ListType(string Split, IType ElementType) : IType
{
    public static readonly ListType Default = new ListType("\n", StringType.Default);

    public string DefaultValue => "";

    public string Type => "list";

    public bool CheckValue(string value)
    {
        return value
            .Split(Split)
            .Select(s => s.Trim())
            .Select(s => ElementType?.CheckValue(s) ?? true)
            .All(s => s);
    }

    public string ValueToString(object value) => string.Join(Split, 
            from object? o in (IEnumerable)value select (ElementType ?? StringType.Default).ValueToString(o));

    public object StringToValue(string value) => value.Split(Split)
            .Select(s => ElementType.StringToValue(s))
            .ToList();
}

/// <summary>
/// 枚举类型
/// </summary>
/// <param name="Type">枚举名</param>
/// <param name="Values">所有枚举</param>
public record EnumType(string Type, List<EnumValue> Values) : IType
{
    private readonly HashSet<string> _values = Values.Select(s => s.Value).ToHashSet();
    private readonly Dictionary<string, EnumValue> _dictionary = Values.ToDictionary(s => s.Value);

    public string DefaultValue => Values[0].Value;

    public bool CheckValue(string value)
    {
        return _values.Contains(value);
    }

    public string ValueToString(object value) => ((EnumValue)value).Value;

    public object StringToValue(string value) => _dictionary.TryGetValue(value, out var v) ? v : Values[0];
}

public record EnumValue(string Value, string Name, string Desc);

public record ArrayType(IType ElementType) : IType
{
    public static readonly ArrayType Default = new(StringType.Default);

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