using Configs.util;

namespace Configs.app;

public class GitAppContext() : AppContext("git")
{
    private const string CmdListAll       = "{0} config --global --list";
    private const string CmdGetValue      = "{0} config --global --get {1}";
    private const string CmdGetAllValue   = "{0} config --global --get-all {1}";
    private const string CmdSetValue      = "{0} config --global {1} {2}";
    private const string CmdAddValue      = "{0} config --global --add {1} {2}";
    private const string CmdUnsetValue    = "{0} config --global --unset {1} {2}";
    private const string CmdUnsetAllValue = "{0} config --global --unset-all {1} {2}";

    public ExeResult GetAllProperties()
    {
        return ExecuteCommand(string.Format(CmdListAll, AppCommand));
    }

    public ExeResult GetGlobalProperty(string key)
    {
        return ExecuteCommand(string.Format(CmdGetValue, AppCommand, key));
    }

    public ExeResult GetAllGlobalProperty(string key)
    {
        return ExecuteCommand(string.Format(CmdGetAllValue, AppCommand, key));
    }

    public ExeResult SetGlobalProperty(string key, string? value)
    {
        return ExecuteCommand(string.Format(CmdSetValue, AppCommand, key, value));
    }

    public ExeResult AddGlobalProperty(string key, string value)
    {
        return ExecuteCommand(string.Format(CmdAddValue, AppCommand, key, value));
    }

    public ExeResult UnsetGlobalProperty(string key, string value)
    {
        return ExecuteCommand(string.Format(CmdUnsetValue, AppCommand, key, value));
    }

    public ExeResult UnsetAllGlobalProperty(string key, string value)
    {
        return ExecuteCommand(string.Format(CmdUnsetAllValue, AppCommand, key, value));
    }

    public override bool ApplyValueToFile(string key, string value)
    {
        return false;
    }
}