using System;

[ChatCommand("v", isGlobal: true)]
public class NumberCommand : IChatCommand
{
    public string Name => "v";
    public object ParseArguments(string argsText)
    {
        // try parse exactly one integer; default to 0 if invalid
        if (int.TryParse(argsText.Trim(), out var choice))
            return choice;
        return 0;
    }
}