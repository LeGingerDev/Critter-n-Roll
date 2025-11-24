using DG.Tweening;
using ScoredProductions.StreamLinked.IRC;
using UnityEngine;

public class LeaveCommandUI : TwitchBaseListener
{
    public override void HandleCommand(string commandKey, object args, string sender, TwitchMessage msg)
    {
        switch (commandKey.ToLowerInvariant())
        {
            case "leave":
                // Remove the player by their Twitch user ID
                var chatData = new ChatUserData(msg);
                long userId = chatData.GetUserId();
                bool removed = ActivePlayerManager.Instance.LeavePlayer(userId);
                if (!removed)
                {
                    Debug.LogWarning($"Could not remove user {userId}; not currently in-game.");
                    return;
                }
                PunchEffect();
                break;
        }
    }

    Tween _punchEffect;

    public void PunchEffect()
    {
        if (_punchEffect != null && _punchEffect.IsActive() && _punchEffect.IsPlaying())
            return;
        _punchEffect = transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 10, 1);
        _punchEffect.OnComplete(() => _punchEffect = null);
    }
}