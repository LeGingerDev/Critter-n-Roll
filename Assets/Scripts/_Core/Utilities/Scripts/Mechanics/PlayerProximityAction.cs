using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
#endif

public class PlayerProximityAction : MonoBehaviour
{
    [SerializeField] private UnityEvent _onPlayerInRange;

    [FoldoutGroup("Settings"), SerializeField]
    private Transform _proximityFrom;

    [FoldoutGroup("Settings"), SerializeField]
    private LayerMask _targetLayers;

    [FoldoutGroup("Settings"), SerializeField]
    private float _spawnDistance;

    private bool _triggered;

    public void Update()
    {
        if (_triggered)
            return;

        if (!Physics2D.OverlapCircle(_proximityFrom.position, _spawnDistance, _targetLayers))
            return;

        _onPlayerInRange?.Invoke();
        _triggered = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_proximityFrom.position, _spawnDistance);

//#if UNITY_EDITOR
//        Handles.color = Color.white;
//        string labelText = "Proximity Player Distance: " + _spawnDistance + "m";
//        Vector3 labelPosition = _proximityFrom.position + Vector3.up * (_spawnDistance + 0.5f); // Adjust as needed
//        Handles.Label(labelPosition, labelText);
//#endif
    }
}