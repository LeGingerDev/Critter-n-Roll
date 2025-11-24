// ChatCommandAttribute.cs
using System;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class ChatCommandAttribute : Attribute
{
    public string CommandName { get; }
    public bool IsGlobal { get; }

    /// <param name="commandName">e.g. "dance", "n", "jump"</param>
    /// <param name="isGlobal">
    ///   true = dispatch to *all* listeners, regardless of username  
    ///   false = only to the listener matching msg.Username
    /// </param>
    public ChatCommandAttribute(string commandName, bool isGlobal = false)
    {
        CommandName = commandName.ToLower();
        IsGlobal = isGlobal;
    }
}
