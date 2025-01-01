using System.Runtime.InteropServices;

namespace Configs.app;

public class DefaultValue(string? windows, string? linux, string? mac)
{
    private IType _type = StringType.Default;
    private string? _windows = windows, _linux = linux, _mac = mac;

    public string Value
    {
        get
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return _windows!;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return _linux!;
            }
            else
            {
                return _mac!;
            }
        }
    }

    public T GetValue<T>()
    {
        return (T)_type.StringToValue(Value);
    }

    public void Fix(IType type)
    {
        _type = type;
        _windows ??= type.DefaultValue;
        _linux ??= type.DefaultValue;
        _mac ??= type.DefaultValue;
    }
}