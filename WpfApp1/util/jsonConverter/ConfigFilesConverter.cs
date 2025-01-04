using Configs.app;
using System.Text.Json.Nodes;

namespace Configs.util.jsonConverter
{
    public class ConfigFilesConverter
    {
        public static Dictionary<string, ConfigFile>? ReadFromJson(JsonNode? n)
        {

            if (n is not JsonObject obj)
            {
                return null;
            }

            Dictionary<string, ConfigFile> files = new();
            foreach (var (name, node) in obj)
            {
                files[name] = ConfigFileConverter.ReadFromJson(node)!;
            }
            return files;
        }
    }
}