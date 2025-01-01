using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Configs.util;

public class FileUtils
{
    public const string AppPath = "apps";
    public const string AppJson = "app.json";
    public const string CommandsJson = "commands.json";
    public const string FilesJson = "files.json";
    public const string FilesDescExt = ".desc.json";
    public const string FilesGroupExt = ".group.json";
    public const string TypesJson = "types.json";

    public static string BuildPath(params string[] paths)
    {
        return Path.Combine([AppDomain.CurrentDomain.BaseDirectory, ..paths]);
    }

    public static List<string> GetConfigList()
    {
        var path = BuildPath(AppPath);
        if (!Directory.Exists(path))
        {
            return [];
        }

        return Directory.GetDirectories(path)
            .Where(s => File.Exists(BuildPath(s, AppJson)))
            .ToList();
    }

    public static ImageSource? LoadImageSource(string dir, string? icon)
    {
        icon ??= "icon.png";
        var path = BuildPath(dir, icon);
        return File.Exists(path) ? new BitmapImage(new Uri(path)) : null;
    }

    public static async Task<T?> ReadJson<T>(string dir, string file, JsonSerializerOptions? options = null)
    {
        var path = BuildPath(dir, file);
        if (!File.Exists(path))
        {
            return default;
        }
        
        Debug.WriteLine($"Json：{typeof(T).FullName} {path}");
        await using var fs = File.OpenRead(path);
        return await JsonSerializer.DeserializeAsync<T>(fs, options);
    }
}