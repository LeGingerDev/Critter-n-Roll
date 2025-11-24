using UnityEngine;

[System.Serializable]
public class SpeedBoostSettings
{
    [SerializeField] private float _boostForce = 10f;
    [SerializeField] private LayerMask _affectedLayers = -1;
    [SerializeField] private ForceMode _forceMode = ForceMode.Force;
    [SerializeField] private Vector3 _boostZoneSize = Vector3.one;
    [SerializeField] private float _updateInterval = 0.02f; // 50fps update rate

    public float GetBoostForce() => _boostForce;
    public LayerMask GetAffectedLayers() => _affectedLayers;
    public ForceMode GetForceMode() => _forceMode;
    public Vector3 GetBoostZoneSize() => _boostZoneSize;
    public float GetUpdateInterval() => _updateInterval;
}