namespace Configs.app;

public class CommandDetail(string? read = null, string? write = null, string? revoke = null)
{
    private string? _read = read, _write = write, _revoke = revoke;

    public string Read { 
        get => _read!; 
        set => _read = value; 
    }

    public string Write { 
        get => _write!; 
        set => _write = value; 
    }

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