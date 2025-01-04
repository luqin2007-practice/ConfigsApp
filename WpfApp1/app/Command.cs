using System.Text.Json.Serialization;

namespace Configs.app;

/// <summary>
///     属性命令
/// </summary>
public class Command
{
    private CommandDetail? _command;
    private DefaultValue? _default;
    private string? _name, _desc;

    /// <summary>
    ///     属性名
    /// </summary>
    [JsonPropertyName("property")]
    public string Property { get; set; } = "";

    /// <summary>
    ///     属性名（显示）
    /// </summary>
    [JsonPropertyName("name")]
    public string Name
    {
        get => _name!;
        set => _name = value;
    }

    /// <summary>
    ///     属性描述
    /// </summary>
    [JsonPropertyName("desc")]
    public string Desc
    {
        get => _desc!;
        set => _desc = value;
    }

    /// <summary>
    ///     属性类型
    /// </summary>
    [JsonPropertyName("type")]
    public IType Type { get; set; } = StringType.Default;

    /// <summary>
    ///     命令集合
    /// </summary>
    [JsonPropertyName("override")]
    public CommandDetail Commands
    {
        get => _command!;
        set => _command = value;
    }

    /// <summary>
    ///     默认值
    /// </summary>
    [JsonPropertyName("default")]
    public DefaultValue Default
    {
        get => _default!;
        set => _default = value;
    }

    public void Fix()
    {
        _name ??= Property;
        _desc ??= Name;
        _default ??= new DefaultValue(Type.DefaultValue, Type.DefaultValue, Type.DefaultValue);
        _default.Fix(Type);
        _command ??= new CommandDetail();
        _command.Fix(Default, Property);
    }
}