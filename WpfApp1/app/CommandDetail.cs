using System.Text.Json.Serialization;

namespace Configs.app;

public class CommandDetail
{
    private string? _read, _write, _revoke;

    [JsonPropertyName("read")]
    public string Read { 
        get => _read!; 
        set => _read = value; 
    }

    [JsonPropertyName("write")]
    public string Write { 
        get => _write!; 
        set => _write = value; 
    }

    [JsonPropertyName("revoke")]
    public string Revoke { 
        get => _revoke!; 
        set => _revoke = value; 
    }

    public void Fix(DefaultValue defValue, string property)
    {
        _read ??= $"{{0}} {property}";
        _write ??= $"{{0}} {property} {{1}}";
        _revoke ??= string.Format(Write, "{0}", defValue.Value);
    }
}