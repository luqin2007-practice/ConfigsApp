namespace Configs.util;

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
    public string ErrorMessage;
    /// <summary>
    /// 是否执行成功
    /// </summary>
    public bool IsOk;

    private ExeResult(string output, string error, bool ok)
    {
        OutputMessage = output;
        ErrorMessage = error;
        IsOk = ok;
    }

    /// <summary>
    /// 创建一个成功的结果
    /// </summary>
    /// <param name="output">输出内容</param>
    public static ExeResult Ok(string output = "")
    {
        return new ExeResult(output, "", true);
    }

    /// <summary>
    /// 创建一个失败的结果
    /// </summary>
    /// <param name="error">异常信息</param>
    public static ExeResult Error(string error)
    {
        return new ExeResult("", error, false);
    }

    /// <summary>
    /// 创建一个失败的结果
    /// </summary>
    /// <param name="output">返回信息</param>
    /// <param name="error">异常信息</param>
    public static ExeResult Error(string output, string error)
    {
        return new ExeResult(output, error, false);
    }
}