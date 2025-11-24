[ChatCommand("o", isGlobal: true)]
public class PollOptionCommand : IChatCommand
{
    public string Name => "o";

    public object ParseArguments(string argsText)
    {
        return argsText.Trim().ToLower();
    }
}