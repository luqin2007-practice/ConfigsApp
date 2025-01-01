using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Configs.app;

public class AppCommands(string? app)
{
    private string? _app = app;

    /// <summary>默认命令行</summary>
    [JsonPropertyName("app")]
    public string App
    {
        get => _app!;
        set => _app = value;
    }

    /// <summary>测试命令行</summary>
    [JsonPropertyName("test")]
    public string Test { get; set; } = "where {}";

    /// <summary>所有命令</summary>
    [JsonPropertyName("commands")]
    public List<Command> Commands { get; set; } = [];

    private readonly Dictionary<string, Command> _commandMap = [];

    /// <summary>
    /// 尝试根据属性名获取属性命令
    /// </summary>
    /// <param name="property">属性名</param>
    /// <param name="command">获取的命令</param>
    /// <returns>是否成功获取了属性</returns>
    public bool TryGetCommand(string property, [MaybeNullWhen(false)] out Command command)
    {
        return _commandMap.TryGetValue(property, out command);
    }

    public void Fix(AppInformation app)
    {
        _app ??= app.Name;
        Commands.ForEach(c => _commandMap[c.Property] = c);
    }
}