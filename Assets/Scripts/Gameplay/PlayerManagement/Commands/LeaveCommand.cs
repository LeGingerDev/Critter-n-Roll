using UnityEngine;

[ChatCommand("leave", true)]
public class LeaveCommand : IChatCommand
{
    public string Name => "leave";

    public object ParseArguments(string argsText)
    {
        return null;
    }
}
