using UnityEngine;

[System.Serializable]
public class ProximitySettings
{
    [SerializeField] private float _detectionRadius = 3f;
    [SerializeField] private LayerMask _targetLayers = -1;
    [SerializeField] private float _checkInterval = 0.1f;

    public float GetDetectionRadius() => _detectionRadius;
    public LayerMask GetTargetLayers() => _targetLayers;
    public float GetCheckInterval() => _checkInterval;
}