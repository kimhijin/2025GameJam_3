using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class EnemyController : Agent
{
    [SerializeField] private int detectionRange = 5;
    [SerializeField] private int maxGridDistance = 4;
    [SerializeField] private float moveInterval = 0.2f;
    [SerializeField] private float idleTimeout = 0.5f;

    private PlayerController player;
    private float moveTimer = 0f;
    private float idleTimer = 0f;
    private bool shouldStopAnimation = false;
    private bool isDead = false;

    private bool stickToVerticalWall = false;
    private bool stickToHorizontalWall = false;

    private SpriteRenderer spriteRenderer;
    private Vector2Int lastMoveDirection = Vector2Int.right;

    protected override void Start()
    {
        base.Start();
        player = FindFirstObjectByType<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastMoveDirection = Vector2Int.right;
    }

    protected override void Update()
    {
        base.Update();

        if (isDead) return;

        bool isHorizontal = (lastMoveDirection.x != 0);
        animator?.SetBool("IsRight", isHorizontal);

        if (player != null)
        {
            int manhattanDistance = GetManhattanDistance(gridPosition, player.GridPosition);
            if (manhattanDistance > detectionRange)
            {
                shouldStopAnimation = true;
                idleTimer = 0f;
                stickToVerticalWall = false;
                stickToHorizontalWall = false;
            }
        }

        if (shouldStopAnimation && !isMoving)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleTimeout)
            {
                animator?.SetBool("IsMoving", false);
                shouldStopAnimation = false;
                idleTimer = 0f;
            }
        }

        if (!isMoving)
        {
            moveTimer += Time.deltaTime;
            if (moveTimer >= moveInterval)
            {
                DecideNextMove();
                moveTimer = 0f;
            }
        }
    }

    private void DecideNextMove()
    {
        if (isDead || player == null) return;

        Vector2Int playerPos = player.GridPosition;
        int manhattanDistance = GetManhattanDistance(gridPosition, playerPos);

        if (manhattanDistance > detectionRange)
        {
            shouldStopAnimation = true;
            idleTimer = 0f;
            return;
        }

        if (stickToVerticalWall)
        {
            FollowAlongVerticalWall(playerPos);
            return;
        }

        if (stickToHorizontalWall)
        {
            FollowAlongHorizontalWall(playerPos);
            return;
        }

        bool sameAxis = gridPosition.x == playerPos.x || gridPosition.y == playerPos.y;
        bool blockedOnAxis = !CanSeePlayerInStraightLine(playerPos);

        if (sameAxis && blockedOnAxis)
        {
            if (MoveTowardWallBetweenUs(playerPos))
                return;

            if (gridPosition.x != playerPos.x && gridPosition.y == playerPos.y)
                stickToVerticalWall = true;
            else if (gridPosition.y != playerPos.y && gridPosition.x == playerPos.x)
                stickToHorizontalWall = true;

            shouldStopAnimation = true;
            idleTimer = 0f;
            return;
        }

        List<Vector2Int> pathToPlayer = FindPathBFS(gridPosition, playerPos);
        if (pathToPlayer == null || pathToPlayer.Count == 0)
        {
            shouldStopAnimation = true;
            idleTimer = 0f;
            return;
        }

        int gridDistance = pathToPlayer.Count - 1;
        if (gridDistance > maxGridDistance)
        {
            MoveCloserToPlayer(playerPos);
            return;
        }

        if (gridDistance == 1)
        {
            MoveWithDirectionPriority(playerPos);
            return;
        }

        MoveAlongPath(pathToPlayer);
    }

    private int GetManhattanDistance(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private bool CanSeePlayerInStraightLine(Vector2Int playerPos)
    {
        if (gridPosition.y == playerPos.y)
            return !IsObstacleInHorizontalLine(gridPosition, playerPos);

        if (gridPosition.x == playerPos.x)
            return !IsObstacleInVerticalLine(gridPosition, playerPos);

        return true;
    }

    private bool IsObstacleInHorizontalLine(Vector2Int from, Vector2Int to)
    {
        int minX = Mathf.Min(from.x, to.x);
        int maxX = Mathf.Max(from.x, to.x);
        int y = from.y;

        for (int x = minX + 1; x < maxX; x++)
        {
            if (IsObstacleAt(new Vector2Int(x, y)))
                return true;
        }
        return false;
    }

    private bool IsObstacleInVerticalLine(Vector2Int from, Vector2Int to)
    {
        int minY = Mathf.Min(from.y, to.y);
        int maxY = Mathf.Max(from.y, to.y);
        int x = from.x;

        for (int y = minY + 1; y < maxY; y++)
        {
            if (IsObstacleAt(new Vector2Int(x, y)))
                return true;
        }
        return false;
    }

    private bool MoveTowardWallBetweenUs(Vector2Int playerPos)
    {
        if (gridPosition.x == playerPos.x)
        {
            int dirY = playerPos.y > gridPosition.y ? 1 : -1;
            Vector2Int step = new Vector2Int(0, dirY);
            Vector2Int nextPos = gridPosition + step;

            if (IsObstacleAt(nextPos))
                return false;

            return MoveWithDirection(step);
        }

        if (gridPosition.y == playerPos.y)
        {
            int dirX = playerPos.x > gridPosition.x ? 1 : -1;
            Vector2Int step = new Vector2Int(dirX, 0);
            Vector2Int nextPos = gridPosition + step;

            if (IsObstacleAt(nextPos))
                return false;

            return MoveWithDirection(step);
        }

        return false;
    }

    private void FollowAlongVerticalWall(Vector2Int playerPos)
    {
        int minX = Mathf.Min(gridPosition.x, playerPos.x);
        int maxX = Mathf.Max(gridPosition.x, playerPos.x);
        
        if (!IsObstacleBetweenColumns(minX, maxX, gridPosition.y))
        {
            stickToVerticalWall = false;
            List<Vector2Int> pathToPlayer = FindPathBFS(gridPosition, playerPos);
            if (pathToPlayer != null && pathToPlayer.Count > 0)
            {
                MoveAlongPath(pathToPlayer);
            }
            return;
        }

        if (playerPos.y > gridPosition.y)
            MoveWithDirection(Vector2Int.up);
        else if (playerPos.y < gridPosition.y)
            MoveWithDirection(Vector2Int.down);
    }

    private void FollowAlongHorizontalWall(Vector2Int playerPos)
    {
        int minY = Mathf.Min(gridPosition.y, playerPos.y);
        int maxY = Mathf.Max(gridPosition.y, playerPos.y);
        
        if (!IsObstacleBetweenRows(minY, maxY, gridPosition.x))
        {
            stickToHorizontalWall = false;
            List<Vector2Int> pathToPlayer = FindPathBFS(gridPosition, playerPos);
            if (pathToPlayer != null && pathToPlayer.Count > 0)
            {
                MoveAlongPath(pathToPlayer);
            }
            return;
        }

        if (playerPos.x > gridPosition.x)
            MoveWithDirection(Vector2Int.right);
        else if (playerPos.x < gridPosition.x)
            MoveWithDirection(Vector2Int.left);
    }

    private bool IsObstacleBetweenColumns(int minX, int maxX, int y)
    {
        for (int x = minX + 1; x < maxX; x++)
        {
            if (IsObstacleAt(new Vector2Int(x, y)))
                return true;
        }
        return false;
    }

    private bool IsObstacleBetweenRows(int minY, int maxY, int x)
    {
        for (int y = minY + 1; y < maxY; y++)
        {
            if (IsObstacleAt(new Vector2Int(x, y)))
                return true;
        }
        return false;
    }

    private List<Vector2Int> FindPathBFS(Vector2Int start, Vector2Int target)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> parent = new Dictionary<Vector2Int, Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        queue.Enqueue(start);
        visited.Add(start);
        parent[start] = start;

        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            if (current == target)
                return ReconstructPath(parent, start, target);

            foreach (var d in dirs)
            {
                Vector2Int nb = current + d;

                if (!GridManager.Instance.IsWalkable(nb))
                    continue;
                if (IsObstacleAt(nb))
                    continue;
                if (visited.Contains(nb))
                    continue;

                visited.Add(nb);
                parent[nb] = current;
                queue.Enqueue(nb);
            }
        }

        return null;
    }

    private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> parent,
                                             Vector2Int start, Vector2Int target)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int cur = target;

        while (cur != start)
        {
            path.Add(cur);
            cur = parent[cur];
        }

        path.Add(start);
        path.Reverse();
        return path;
    }

    private void MoveAlongPath(List<Vector2Int> path)
    {
        if (path.Count < 2)
            return;

        Vector2Int nextStep = path[1];
        Vector2Int dir = nextStep - gridPosition;

        MoveWithDirection(dir);
    }

    private void MoveCloserToPlayer(Vector2Int playerPos)
    {
        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        Vector2Int bestMove = Vector2Int.zero;
        int bestDist = GetManhattanDistance(gridPosition, playerPos);
        bool found = false;

        foreach (var d in dirs)
        {
            Vector2Int np = gridPosition + d;

            if (!GridManager.Instance.IsWalkable(np))
                continue;
            if (IsObstacleAt(np))
                continue;
            if (GridManager.Instance.GetOccupier(np) != null)
                continue;

            int dist = GetManhattanDistance(np, playerPos);
            if (dist < bestDist)
            {
                bestDist = dist;
                bestMove = d;
                found = true;
            }
        }

        if (found)
        {
            MoveWithDirection(bestMove);
        }
    }

    private void MoveWithDirectionPriority(Vector2Int playerPos)
    {
        Vector2Int diff = playerPos - gridPosition;
        Vector2Int primary;

        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
            primary = new Vector2Int(diff.x > 0 ? 1 : -1, 0);
        else
            primary = new Vector2Int(0, diff.y > 0 ? 1 : -1);

        if (TryMoveAsEnemy(primary))
            return;

        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (var d in dirs)
        {
            if (d == primary)
                continue;

            if (TryMoveAsEnemy(d))
                return;
        }
    }

    private bool TryMoveAsEnemy(Vector2Int direction)
    {
        if (isMoving)
            return false;

        Vector2Int newPos = gridPosition + direction;

        if (!GridManager.Instance.IsWalkable(newPos))
            return false;

        GameObject occupier = GridManager.Instance.GetOccupier(newPos);
        if (occupier != null)
        {       
            IKillable killable = occupier.GetComponent<IKillable>();
            if (killable != null)
            {
                MoveWithDirection(direction);
                killable.Dead();
                return true;
            }
            return false;
        }

        if (IsObstacleAt(newPos))
            return false;

        return MoveWithDirection(direction);
    }

    private bool MoveWithDirection(Vector2Int direction)
    {
        if (isMoving) return false;

        Vector2Int newPos = gridPosition + direction;
        if (!GridManager.Instance.IsWalkable(newPos)) return false;
        if (IsObstacleAt(newPos)) return false;

        lastMoveDirection = direction;

        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = (direction.x < 0);
            bool isUpDown = (direction.y != 0);
            if (isUpDown)
                spriteRenderer.flipY = (direction.y > 0);
        }

        animator?.SetBool("IsMoving", true);
        shouldStopAnimation = false;
        idleTimer = 0f;

        StartCoroutine(MoveToCell(newPos));
        return true;
    }

    protected override void OnMoveComplete()
    {
        shouldStopAnimation = true;
        idleTimer = 0f;
        if (player != null)
        {
            Vector2Int playerPos = player.GridPosition;
            Vector2Int dirToPlayer = playerPos - gridPosition;
            
            if (Mathf.Abs(dirToPlayer.x) > Mathf.Abs(dirToPlayer.y))
            {
                lastMoveDirection = new Vector2Int(dirToPlayer.x > 0 ? 1 : -1, 0);
            }
            else
            {
                lastMoveDirection = new Vector2Int(0, dirToPlayer.y > 0 ? 1 : -1);
            }
            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = (lastMoveDirection.x < 0);
                spriteRenderer.flipY = (lastMoveDirection.y > 0);
            }
            bool isHorizontal = (lastMoveDirection.x != 0);
            animator?.SetBool("IsRight", isHorizontal);
        }
    }

    public override void Dead()
    {
        isDead = true;
        StartCoroutine(Co_StylishDeath());
    }

    private IEnumerator Co_StylishDeath()
    {
        float duration = 0.6f;
        float t = 0f;
        SpriteRenderer sr = spriteRenderer;
        Color startColor = sr.color;
        Vector3 startScale = transform.localScale;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float lerp = t / duration;

            transform.localScale = Vector3.Lerp(startScale, startScale * 0.2f, lerp);
            sr.color = new Color(startColor.r, startColor.g, startColor.b, 1f - lerp);

            yield return null;
        }
        Destroy(gameObject);
    }
}
