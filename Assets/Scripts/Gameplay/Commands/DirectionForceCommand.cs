using System;
using System.Collections.Generic;
using UnityEngine;

[ChatCommand("d")]
public class DirectionForceCommand : IChatCommand
{
    public string Name => "d";

    /// <summary>Maps directional strings to their corresponding angles in degrees</summary>
    private static readonly Dictionary<string, float> _directionAngles = new Dictionary<string, float>(StringComparer.OrdinalIgnoreCase)
    {
        { "N", 0f },
        { "NE", 45f },
        { "E", 90f },
        { "SE", 135f },
        { "S", 180f },
        { "SW", 225f },
        { "W", 270f },
        { "NW", 315f },
        { "Up", 0f },
        { "Right", 90f },
        { "Down", 180f },
        { "Left", 270f }
    };

    public object ParseArguments(string argsText)
    {
        // split on whitespace
        var tokens = argsText
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        // defaults
        float angle = 0f;
        float force = 1f;

        if (tokens.Length == 0)
            return new DirectionArguments(angle, force);

        // check if first token is a directional string
        if (_directionAngles.TryGetValue(tokens[0], out var directionAngle))
        {
            angle = directionAngle;

            // look for force in remaining tokens
            force = ParseForceFromTokens(tokens, 1);
        }
        else
        {
            // first token should be a number (angle)
            if (float.TryParse(tokens[0], out var aVal))
                angle = aVal;

            // look for force in remaining tokens
            force = ParseForceFromTokens(tokens, 1);
        }

        return new DirectionArguments(angle, force);
    }

    /// <summary>Parses force value from tokens starting at the specified index</summary>
    private float ParseForceFromTokens(string[] tokens, int startIndex)
    {
        float force = 1f;

        // look for an explicit "f" marker or a second number
        for (int i = startIndex; i < tokens.Length; i++)
        {
            var t = tokens[i];
            if (t.Equals("f", StringComparison.OrdinalIgnoreCase) && i + 1 < tokens.Length
                && float.TryParse(tokens[i + 1], out var fVal))
            {
                force = fVal;
                break;
            }
            else if (float.TryParse(t, out var fVal2))
            {
                force = fVal2;
                break;
            }
        }

        return force;
    }
}

[ChatCommand("m")]
public class DirectionMForceCommand : IChatCommand
{
    public string Name => "m";

    /// <summary>Maps directional strings to their corresponding angles in degrees</summary>
    private static readonly Dictionary<string, float> _directionAngles = new Dictionary<string, float>(StringComparer.OrdinalIgnoreCase)
    {
        { "N", 0f },
        { "NE", 45f },
        { "E", 90f },
        { "SE", 135f },
        { "S", 180f },
        { "SW", 225f },
        { "W", 270f },
        { "NW", 315f },
        { "Up", 0f },
        { "Right", 90f },
        { "Down", 180f },
        { "Left", 270f }
    };

    public object ParseArguments(string argsText)
    {
        // split on whitespace
        var tokens = argsText
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        // defaults
        float angle = 0f;
        float force = 1f;

        if (tokens.Length == 0)
            return new DirectionArguments(angle, force);

        // check if first token is a directional string
        if (_directionAngles.TryGetValue(tokens[0], out var directionAngle))
        {
            angle = directionAngle;

            // look for force in remaining tokens
            force = ParseForceFromTokens(tokens, 1);
        }
        else
        {
            // first token should be a number (angle)
            if (float.TryParse(tokens[0], out var aVal))
                angle = aVal;

            // look for force in remaining tokens
            force = ParseForceFromTokens(tokens, 1);
        }

        return new DirectionArguments(angle, force);
    }

    /// <summary>Parses force value from tokens starting at the specified index</summary>
    private float ParseForceFromTokens(string[] tokens, int startIndex)
    {
        float force = 1f;

        // look for an explicit "f" marker or a second number
        for (int i = startIndex; i < tokens.Length; i++)
        {
            var t = tokens[i];
            if (t.Equals("f", StringComparison.OrdinalIgnoreCase) && i + 1 < tokens.Length
                && float.TryParse(tokens[i + 1], out var fVal))
            {
                force = fVal;
                break;
            }
            else if (float.TryParse(t, out var fVal2))
            {
                force = fVal2;
                break;
            }
        }

        return force;
    }
}

[Serializable]
public class DirectionArguments
{
    /// <summary>Clamped to [0,360]</summary>
    public float angle { get; }
    /// <summary>Clamped to [0,1]</summary>
    public float force { get; }

    public DirectionArguments(float angle, float force)
    {
        // clamp angle between 0° and 360°
        this.angle = Mathf.Clamp(angle, 0f, 360f);
        // clamp force between 0 and 1
        this.force = Mathf.Clamp(force, 0f, 10f);
    }
}

/// <summary>Base class for directional commands that have a fixed direction and only parse force</summary>
public abstract class BaseDirectionalCommand : IChatCommand
{
    protected abstract string CommandName { get; }
    protected abstract float DirectionAngle { get; }

    public string Name => CommandName;

    public object ParseArguments(string argsText)
    {
        float force = ParseForceFromText(argsText);
        return new DirectionArguments(DirectionAngle, force);
    }

    /// <summary>Parses force value from the argument text</summary>
    private float ParseForceFromText(string argsText)
    {
        if (string.IsNullOrWhiteSpace(argsText))
            return 10f;

        var tokens = argsText.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        // look for an explicit "f" marker or just a number
        for (int i = 0; i < tokens.Length; i++)
        {
            var token = tokens[i];

            // check for "f 0.5" pattern
            if (token.Equals("f", StringComparison.OrdinalIgnoreCase) && i + 1 < tokens.Length
                && float.TryParse(tokens[i + 1], out var fVal))
            {
                return fVal;
            }
            // check for direct number
            else if (float.TryParse(token, out var directVal))
            {
                return directVal;
            }
        }

        return 1f; // default force
    }
}

// North Commands
[ChatCommand("n")]
public class NorthCommand : BaseDirectionalCommand
{
    protected override string CommandName => "n";
    protected override float DirectionAngle => 0f;
}

[ChatCommand("ne")]
public class NorthEastCommand : BaseDirectionalCommand
{
    protected override string CommandName => "ne";
    protected override float DirectionAngle => 45f;
}

[ChatCommand("e")]
public class EastCommand : BaseDirectionalCommand
{
    protected override string CommandName => "e";
    protected override float DirectionAngle => 90f;
}

[ChatCommand("se")]
public class SouthEastCommand : BaseDirectionalCommand
{
    protected override string CommandName => "se";
    protected override float DirectionAngle => 135f;
}

[ChatCommand("s")]
public class SouthCommand : BaseDirectionalCommand
{
    protected override string CommandName => "s";
    protected override float DirectionAngle => 180f;
}

[ChatCommand("sw")]
public class SouthWestCommand : BaseDirectionalCommand
{
    protected override string CommandName => "sw";
    protected override float DirectionAngle => 225f;
}

[ChatCommand("w")]
public class WestCommand : BaseDirectionalCommand
{
    protected override string CommandName => "w";
    protected override float DirectionAngle => 270f;
}

[ChatCommand("nw")]
public class NorthWestCommand : BaseDirectionalCommand
{
    protected override string CommandName => "nw";
    protected override float DirectionAngle => 315f;
}

// Alternative naming (Up, Down, Left, Right)
[ChatCommand("up")]
public class UpCommand : BaseDirectionalCommand
{
    protected override string CommandName => "up";
    protected override float DirectionAngle => 0f;
}

[ChatCommand("right")]
public class RightCommand : BaseDirectionalCommand
{
    protected override string CommandName => "right";
    protected override float DirectionAngle => 90f;
}

[ChatCommand("down")]
public class DownCommand : BaseDirectionalCommand
{
    protected override string CommandName => "down";
    protected override float DirectionAngle => 180f;
}

[ChatCommand("left")]
public class LeftCommand : BaseDirectionalCommand
{
    protected override string CommandName => "left";
    protected override float DirectionAngle => 270f;
}