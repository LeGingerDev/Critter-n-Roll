public interface IPlayerView
{
    /// <summary>
    /// Called once right after Spawn so the view can display the owner's username, etc.
    /// </summary>
    void Initialize(PlayerUserData username);
}