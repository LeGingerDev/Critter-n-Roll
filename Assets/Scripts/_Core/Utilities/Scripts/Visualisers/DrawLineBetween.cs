using Sirenix.OdinInspector;
using UnityEngine;

namespace Utilities.Visualisers
{
    [RequireComponent(typeof(LineRenderer))]
    public class DrawLineBetween : MonoBehaviour
    {
        [FoldoutGroup("References")] [SerializeField]
        private LineRenderer _lineRenderer;

        [FoldoutGroup("References")] [SerializeField]
        private Transform _startTransform;

        [FoldoutGroup("References")] [SerializeField]
        private Transform _endTransform;

        [FoldoutGroup("Settings")] [SerializeField]
        private bool _updateContinuously;


        private void Awake()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.useWorldSpace = true;

            if (_startTransform == null || _endTransform == null)
            {
                Debug.LogError("Transforms are not set.");
                return;
            }

            DrawLine();
        }

        private void Update()
        {
            if (!_updateContinuously)
                return;

            DrawLine();
        }

        [Button("Force Update Line")]
        private void DrawLine()
        {
            if (_lineRenderer == null || _startTransform == null || _endTransform == null)
            {
                return;
            }

            _lineRenderer.SetPosition(0, _startTransform.position);
            _lineRenderer.SetPosition(1, _endTransform.position);
        }

        private void OnValidate()
        {
            _lineRenderer = GetComponent<LineRenderer>();

            if (_lineRenderer == null || _startTransform == null || _endTransform == null)
            {
                return;
            }

            _lineRenderer.SetPosition(0, _startTransform.position);
            _lineRenderer.SetPosition(1, _endTransform.position);
        }
    }
}