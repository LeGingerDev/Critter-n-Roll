[ChatCommand("join", isGlobal: true)]
public class JoinCommand : IChatCommand
{
    public string Name => "join";
    public object ParseArguments(string argsText) => null;
}