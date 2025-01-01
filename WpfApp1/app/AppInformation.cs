using System.IO;
using System.Text.Json.Serialization;

namespace Configs.app;

public class AppInformation
{
    private string? _name;
    
    [JsonPropertyName("name")]
    public string Name
    {
        get => _name!; 
        set => _name = value;
    }
    
    [JsonPropertyName("icon")]
    public string? Icon { get; set; } = null;
    
    [JsonPropertyName("hasPresets")]
    public bool HasPresets { get; set; } = true;

    public void Fix(string dir)
    {
        _name ??= Path.GetDirectoryName(_name);
    }
}