using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Configs.util;

public class FileUtils
{
    public static string BuildPath(params string[] paths)
    {
        return Path.Combine(AppDomain.CurrentDomain.BaseDirectory + paths);
    }

    public static List<string> GetConfigList()
    {
        var path = BuildPath("configs");
        if (!Directory.Exists(path))
        {
            return [];
        }
        return Directory.GetFiles(path)
            // 仅保留包含 app.json 的文件夹
            .Where(s => Directory.Exists(BuildPath("configs", s)))
            .Where(s => File.Exists(BuildPath("configs", s, "app.json")))
            .ToList();
    }

    public static App ReadConfig(string name)
    {
        var path = BuildPath("configs", name + ".json");
        if (!File.Exists(path))
        {
            MessageBox.Show($"{path} 配置文件不存在");
        }

        var fs = File.OpenRead(path);
        var c = JsonSerializer.Deserialize<App>(fs);
        fs.Close();
        if (string.IsNullOrEmpty(c!.Name))
        {
            c.Name = name;
        }
        return c;
    }

    public static ImageSource ReadImage(string dir, string icon)
    {
        var path = BuildPath("configs", dir, icon);
        if (!File.Exists(path))
        {
            MessageBox.Show($"{path} 不存在");
        }
        return new BitmapImage(new Uri(path));
    }
}