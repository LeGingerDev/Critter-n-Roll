[ChatCommand("spin", false)]
public class SpinCommand : IChatCommand
{
    public string Name => "spin";
    public object ParseArguments(string argsText) => null;
}