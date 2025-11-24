using Core;
using Sirenix.OdinInspector;
using UnityEngine;

public class LevelGrid : BaseBehaviour
{
    [FoldoutGroup("Grid Visualization")]
    [SerializeField] private bool _showGrid = true;

    [FoldoutGroup("Grid Visualization")]
    [InfoBox("Only odd numbers work: 1=center square, 3=center+ring, 5=center+2rings, etc.")]
    [SerializeField, Min(1)] private int _gridRingSize = 7;

    [FoldoutGroup("Grid Visualization")]
    [SerializeField, Min(0.1f)] private float _cellSize = 3f;

    [FoldoutGroup("Grid Visualization")]
    [SerializeField] private Color _gridColor = Color.white;

    [FoldoutGroup("Grid Visualization")]
    [SerializeField] private Color _centerColor = Color.red;

    [FoldoutGroup("Grid Visualization")]
    [SerializeField] private bool _showCenterPoint = true;

    public bool ShowGrid => _showGrid;
    public int GridRingSize => GetValidRingSize();
    public float CellSize => _cellSize;

    private void OnDrawGizmos()
    {
        if (!_showGrid) return;

        int validRingSize = GetValidRingSize();
        if (validRingSize <= 0) return;

        DrawGrid(validRingSize);

        if (_showCenterPoint)
        {
            DrawCenterPoint();
        }
    }

    private int GetValidRingSize()
    {
        // Only allow odd numbers, if even number is entered, don't draw anything
        return _gridRingSize % 2 == 1 ? _gridRingSize : 0;
    }

    private void DrawGrid(int ringSize)
    {
        Gizmos.color = _gridColor;
        Vector3 centerPosition = transform.position;

        // For a ringSize×ringSize grid, we need (ringSize + 1) lines in each direction
        int numLines = ringSize + 1;
        float halfExtent = (ringSize * _cellSize) / 2f;

        // Draw vertical lines (parallel to Z-axis)
        for (int i = 0; i < numLines; i++)
        {
            float x = -halfExtent + (i * _cellSize);
            Vector3 start = centerPosition + new Vector3(x, 0, -halfExtent);
            Vector3 end = centerPosition + new Vector3(x, 0, halfExtent);
            Gizmos.DrawLine(start, end);
        }

        // Draw horizontal lines (parallel to X-axis)  
        for (int i = 0; i < numLines; i++)
        {
            float z = -halfExtent + (i * _cellSize);
            Vector3 start = centerPosition + new Vector3(-halfExtent, 0, z);
            Vector3 end = centerPosition + new Vector3(halfExtent, 0, z);
            Gizmos.DrawLine(start, end);
        }
    }

    private void DrawCenterPoint()
    {
        Gizmos.color = _centerColor;
        Vector3 centerPosition = transform.position;

        // Draw center cross
        float crossSize = _cellSize * 0.3f;
        Gizmos.DrawLine(centerPosition + Vector3.left * crossSize, centerPosition + Vector3.right * crossSize);
        Gizmos.DrawLine(centerPosition + Vector3.forward * crossSize, centerPosition + Vector3.back * crossSize);

        // Draw center cube
        Gizmos.DrawWireCube(centerPosition, Vector3.one * (_cellSize * 0.1f));
    }

    // Helper function to get world position from grid coordinates
    // Grid coordinates: (0,0) is center, (-1,-1) is bottom-left of 3x3, etc.
    public Vector3 GetWorldPositionFromGridCoordinates(int gridX, int gridZ)
    {
        return transform.position + new Vector3(gridX * _cellSize, 0, gridZ * _cellSize);
    }

    // Helper function to get grid coordinates from world position
    public Vector2Int GetGridCoordinatesFromWorldPosition(Vector3 worldPosition)
    {
        Vector3 localPosition = worldPosition - transform.position;
        int gridX = Mathf.RoundToInt(localPosition.x / _cellSize);
        int gridZ = Mathf.RoundToInt(localPosition.z / _cellSize);
        return new Vector2Int(gridX, gridZ);
    }

    // Helper function to check if coordinates are within current grid bounds
    public bool IsWithinGridBounds(int gridX, int gridZ)
    {
        int validRingSize = GetValidRingSize();
        if (validRingSize <= 0) return false;

        int halfRange = (validRingSize - 1) / 2;
        return gridX >= -halfRange && gridX <= halfRange && gridZ >= -halfRange && gridZ <= halfRange;
    }

    // Helper function to get all valid grid positions for current ring size
    public Vector2Int[] GetAllGridPositions()
    {
        int validRingSize = GetValidRingSize();
        if (validRingSize <= 0) return new Vector2Int[0];

        int totalCells = validRingSize * validRingSize;
        Vector2Int[] positions = new Vector2Int[totalCells];
        int index = 0;

        int halfRange = (validRingSize - 1) / 2;

        for (int x = -halfRange; x <= halfRange; x++)
        {
            for (int z = -halfRange; z <= halfRange; z++)
            {
                positions[index] = new Vector2Int(x, z);
                index++;
            }
        }

        return positions;
    }
}