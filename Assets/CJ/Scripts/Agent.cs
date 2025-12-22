using UnityEngine;
using System.Collections;

public abstract class Agent : MonoBehaviour
{
    [SerializeField] protected float moveDuration = 0.2f;

    protected Vector2Int gridPosition;
    protected Animator animator;
    public Vector2Int GridPosition 
    { 
        get => gridPosition;
        set => gridPosition = value;
    }

    protected bool isMoving = false;
    public bool IsMoving => isMoving;

    [SerializeField] protected LayerMask unwalkableLayer;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        gridPosition = GridManager.Instance.WorldToGridPosition(transform.position);
        GridManager.Instance.SetCellOccupied(gridPosition, gameObject);
    }

    protected virtual void Update() { }

    public virtual bool TryMove(Vector2Int direction)
    {
        if (isMoving)
            return false;

        Vector2Int newPosition = gridPosition + direction;

        if (!GridManager.Instance.IsWalkable(newPosition))
            return false;

        if (IsObstacleAt(newPosition))
            return false;

        GameObject occupier = GridManager.Instance.GetOccupier(newPosition);
        if (occupier != null)
            return false;

        StartCoroutine(MoveToCell(newPosition));
        return true;
    }

    protected IEnumerator MoveToCell(Vector2Int newGridPos)
    {
        isMoving = true;

        Vector3 startPos = transform.position;
        Vector3 targetPos = GridManager.Instance.GridToWorldPosition(newGridPos);

        GridManager.Instance.ClearCell(gridPosition);
        gridPosition = newGridPos;
        GridManager.Instance.SetCellOccupied(gridPosition, gameObject);

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / moveDuration);
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
        
        OnMoveComplete();  // 이동 완료 신호
    }

    protected virtual void OnMoveComplete()  // ← virtual 추가
    {
        // 기본적으로 아무것도 안 함
    }

    protected bool IsObstacleAt(Vector2Int gridPos)
    {
        Vector3 worldPos = GridManager.Instance.GridToWorldPosition(gridPos);
        Collider2D hit = Physics2D.OverlapCircle(worldPos, 0.1f, unwalkableLayer);
        return hit != null;
    }
}
