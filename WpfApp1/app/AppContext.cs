using System.Diagnostics;
using Configs.util;

namespace Configs.app;

/// <summary>
/// 应用程序上下文
/// </summary>
public abstract class AppContext(string appCommand)
{
    public string AppCommand = appCommand;

    public abstract bool ApplyValueToFile(string key, string value);

    public ExeResult ExecuteCommand(string command)
    {
        var process = new Process();
        var startInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            // 执行命令并退出
            Arguments = "/c " + command,
        };
        process.StartInfo = startInfo;
        process.Start();
        var errMessage = process.StandardError.ReadToEnd();
        var outMessage = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return errMessage != ""
            ? ExeResult.Error(outMessage, errMessage)
            : ExeResult.Ok(outMessage);
    }
}