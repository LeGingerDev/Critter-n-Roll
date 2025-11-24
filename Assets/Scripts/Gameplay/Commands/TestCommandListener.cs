
using ScoredProductions.StreamLinked.IRC;
using UnityEngine;

public class TestCommandListener : TwitchBaseListener
{
    public override void HandleCommand(string commandKey, object args, string sender, TwitchMessage msg)
    {
        switch(commandKey)
        {
            case "n":
                Debug.Log($"{sender} selected {(int)args} as their number");
                break;
        }
    }
}
