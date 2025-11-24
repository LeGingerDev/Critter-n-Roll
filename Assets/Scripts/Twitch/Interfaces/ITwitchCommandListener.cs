// ITwitchCommandListener.cs
using ScoredProductions.StreamLinked.IRC;

public interface ITwitchCommandListener
{
    bool CanHandleUser(TwitchMessage msg);
    void HandleCommand(string commandKey, object args, string senderName, TwitchMessage twitchMessage);
}
