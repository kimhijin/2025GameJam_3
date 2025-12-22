using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [SerializeField] public int gridWidth = 16;
    [SerializeField] public int gridHeight = 9;
    [SerializeField] public float cellSize = 1f;

    [SerializeField] private bool showGridGizmo = true;
    [SerializeField] private Color gridColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    [SerializeField] private Color obstacleColor = new Color(1f, 0f, 0f, 0.3f);
    [SerializeField] private LayerMask unwalkableLayer;

    private Dictionary<Vector2Int, GameObject> occupiedCells = new Dictionary<Vector2Int, GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public bool IsWalkable(Vector2Int position)
    {
        return position.x >= 0 && position.x < gridWidth &&
               position.y >= 0 && position.y < gridHeight;
    }

    public Vector3 GridToWorldPosition(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x * cellSize, gridPos.y * cellSize, 0f);
    }

    public Vector2Int WorldToGridPosition(Vector3 worldPos)
    {
        return new Vector2Int(
            Mathf.RoundToInt(worldPos.x / cellSize),
            Mathf.RoundToInt(worldPos.y / cellSize)
        );
    }

    public bool SetCellOccupied(Vector2Int position, GameObject occupier)
    {
        if (!IsWalkable(position))
            return false;

        occupiedCells[position] = occupier;
        return true;
    }

    public void ClearCell(Vector2Int position)
    {
        if (occupiedCells.ContainsKey(position))
            occupiedCells.Remove(position);
    }

    public GameObject GetOccupier(Vector2Int position)
    {
        occupiedCells.TryGetValue(position, out GameObject occupier);
        return occupier;
    }

    private void OnDrawGizmos()
    {
        if (!showGridGizmo)
            return;

        DrawGridLines();
        DrawObstacleCells();
    }

    private void DrawGridLines()
    {
        Gizmos.color = gridColor;

        for (int x = 0; x <= gridWidth; x++)
        {
            Vector3 start = GridToWorldPosition(new Vector2Int(x, 0));
            Vector3 end = GridToWorldPosition(new Vector2Int(x, gridHeight));
            Gizmos.DrawLine(start, end);
        }

        for (int y = 0; y <= gridHeight; y++)
        {
            Vector3 start = GridToWorldPosition(new Vector2Int(0, y));
            Vector3 end = GridToWorldPosition(new Vector2Int(gridWidth, y));
            Gizmos.DrawLine(start, end);
        }
    }

    private void DrawObstacleCells()
    {
        Gizmos.color = obstacleColor;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector2Int gridPos = new Vector2Int(x, y);
                Vector3 worldPos = GridToWorldPosition(gridPos);

                Collider2D hit = Physics2D.OverlapCircle(worldPos, 0.1f, unwalkableLayer);
                if (hit != null)
                {
                    Vector3 size = new Vector3(cellSize, cellSize, 0.01f);
                    Gizmos.DrawCube(worldPos, size);
                }
            }
        }
    }
}
