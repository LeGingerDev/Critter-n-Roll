// IChatCommand.cs
using System;

public interface IChatCommand
{
    /// <summary>e.g. "dance", "d", "jump"</summary>
    string Name { get; }
    /// <summary>
    /// Parse everything after the command keyword into a simple object.
    /// Return null if no arguments are needed.
    /// </summary>
    object ParseArguments(string argsText);
}