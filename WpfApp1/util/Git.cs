namespace Configs.util;

public class Git(string appCommand = "git")
{
    private const string CmdListAll = "{0} config --global --list";
    private const string CmdGetValue = "{0} config --global --get {1}";
    private const string CmdGetAllValue = "{0} config --global --get-all {1}";
    private const string CmdSetValue = "{0} config --global {1} {2}";
    private const string CmdAddValue = "{0} config --global --add {1} {2}";
    private const string CmdUnsetValue = "{0} config --global --unset {1} {2}";
    private const string CmdUnsetAllValue = "{0} config --global --unset-all {1} {2}";

    public ExeResult GetAllProperties()
    {
        return CommandUtils.Execute(string.Format(CmdListAll, appCommand));
    }

    public ExeResult GetGlobalProperty(string key)
    {
        return CommandUtils.Execute(string.Format(CmdGetValue, appCommand, key));
    }

    public ExeResult GetAllGlobalProperty(string key)
    {
        return CommandUtils.Execute(string.Format(CmdGetAllValue, appCommand, key));
    }

    public ExeResult SetGlobalProperty(string key, string? value)
    {
        return CommandUtils.Execute(string.Format(CmdSetValue, appCommand, key, value));
    }

    public ExeResult AddGlobalProperty(string key, string value)
    {
        return CommandUtils.Execute(string.Format(CmdAddValue, appCommand, key, value));
    }

    public ExeResult UnsetGlobalProperty(string key, string value)
    {
        return CommandUtils.Execute(string.Format(CmdUnsetValue, appCommand, key, value));
    }

    public ExeResult UnsetAllGlobalProperty(string key, string value)
    {
        return CommandUtils.Execute(string.Format(CmdUnsetAllValue, appCommand, key, value));
    }
}