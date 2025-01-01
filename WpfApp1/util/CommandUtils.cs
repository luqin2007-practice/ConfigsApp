using System.Diagnostics;
using Configs.widget;

namespace Configs.util;

public static class CommandUtils
{
    /// <summary>
    /// 执行一个命令
    /// </summary>
    /// <param name="command">命令行内容</param>
    /// <returns>运行结果</returns>
    public static ExeResult Execute(string command)
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
        return string.IsNullOrEmpty(errMessage)
            ? ExeResult.Ok(outMessage)
            : ExeResult.Error(errMessage, outMessage);
    }

    public static ExeResult Execute(string command, params object?[] args)
    {
        return Execute(string.Format(command, args));
    }

    public static Error? ToError(this ExeResult e, string name)
    {
        return e.IsOk ? null : new Error(name, e.ErrorMessage);
    }
}

/// <summary>
/// 表示一个命令执行结果
/// </summary>
public class ExeResult
{
    /// <summary>
    /// 输出信息
    /// </summary>
    public string OutputMessage;
    /// <summary>
    /// 错误信息
    /// </summary>
    public string? ErrorMessage;
    /// <summary>
    /// 是否执行成功
    /// </summary>
    public bool IsOk;

    private ExeResult(string output, string? error, bool ok)
    {
        OutputMessage = output;
        ErrorMessage = error;
        IsOk = ok;

        if (IsOk && string.IsNullOrEmpty(error))
        {
            ErrorMessage = null;
        }
    }

    /// <summary>
    /// 创建一个成功的结果
    /// </summary>
    /// <param name="output">输出内容</param>
    public static ExeResult Ok(string output = "")
    {
        return new ExeResult(output, null, true);
    }

    /// <summary>
    /// 创建一个失败的结果
    /// </summary>
    /// <param name="output">返回信息</param>
    /// <param name="error">异常信息</param>
    public static ExeResult Error(string error, string output = "")
    {
        return new ExeResult(output, error, false);
    }
}