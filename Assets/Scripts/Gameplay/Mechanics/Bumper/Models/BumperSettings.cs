using DG.Tweening;
using UnityEngine;

[System.Serializable]
public class BumperSettings
{
    [SerializeField] private float _bounceForce = 15f;
    [SerializeField] private LayerMask _affectedLayers = -1;
    [SerializeField] private float _punchScale = 1.2f;
    [SerializeField] private float _punchDuration = 0.2f;
    [SerializeField] private Ease _punchEase = Ease.OutBack;

    public float GetBounceForce() => _bounceForce;
    public LayerMask GetAffectedLayers() => _affectedLayers;
    public float GetPunchScale() => _punchScale;
    public float GetPunchDuration() => _punchDuration;
    public Ease GetPunchEase() => _punchEase;
}