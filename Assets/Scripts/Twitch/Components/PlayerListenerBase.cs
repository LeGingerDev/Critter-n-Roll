public abstract class PlayerListenerBase : TwitchBaseListener, IPlayerView
{
    private bool _initialized = false;

    /// <summary>
    /// Called by PlayerSpawnManager (or any IPlayerView initializer) once after Instantiate.
    /// </summary>
    public void Initialize(PlayerUserData userData)
    {
        if (_initialized) return;
        _initialized = true;

        base.InitializeForUser(userData);
    }
}