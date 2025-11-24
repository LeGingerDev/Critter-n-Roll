using UnityEngine;

[System.Serializable]
public class ExplosionSettings
{
    [SerializeField] private float _tickDuration = 3f;
    [SerializeField] private float _explosionForce = 1000f;
    [SerializeField] private float _explosionRadius = 5f;
    [SerializeField] private LayerMask _affectedLayers = -1;
    [SerializeField] private string _shaderPropertyName = "_TextureImpact";

    public float GetTickDuration() => _tickDuration;
    public float GetExplosionForce() => _explosionForce;
    public float GetExplosionRadius() => _explosionRadius;
    public LayerMask GetAffectedLayers() => _affectedLayers;
    public string GetShaderPropertyName() => _shaderPropertyName;
}