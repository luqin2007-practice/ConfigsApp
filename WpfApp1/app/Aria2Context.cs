namespace Configs.app;

public class Aria2Context(string appCommand) : AppContext(appCommand)
{
    public override bool ApplyValueToFile(string key, string value)
    {
        throw new NotImplementedException();
    }
}