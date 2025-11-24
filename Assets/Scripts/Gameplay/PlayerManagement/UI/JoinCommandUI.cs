using DG.Tweening;
using ScoredProductions.StreamLinked.IRC;
using UnityEngine;

public class JoinCommandUI : TwitchBaseListener
{
    [SerializeField]
    private bool _canJoin = false;

    public GameObject _activevisual;
    public GameObject _inactiveVisual;

    private void Start()
    {
        ToggleJoinVisual(false);
    }

    public override void HandleCommand(string commandKey, object args, string sender, TwitchMessage msg)
    {
        if(!_canJoin)
            return;

        switch (commandKey.ToLowerInvariant())
        {
            case "join":
                ActivePlayerManager.Instance.JoinPlayer(msg);
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

    public void ToggleJoinVisual(bool canJoin)
    {
        _canJoin = canJoin;
        _activevisual.SetActive(canJoin);
        _inactiveVisual.SetActive(!canJoin);
    }
}