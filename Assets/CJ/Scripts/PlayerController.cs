using UnityEngine;

public class PlayerController : Agent
{
    [SerializeField] private float moveInterval = 0.2f;

    private float moveTimer = 0f;
    private Vector2Int bufferedDirection = Vector2Int.zero;

    public int itemsCollected { get; private set; } = 0;

    protected override void Update()
    {
        base.Update();

        ReadInputToBuffer();

        if (!isMoving)
        {
            moveTimer += Time.deltaTime;

            if (moveTimer >= moveInterval)
            {
                if (bufferedDirection != Vector2Int.zero)
                    TryMove(bufferedDirection);

                bufferedDirection = Vector2Int.zero;
                moveTimer = 0f;
            }
        }
    }

    private void ReadInputToBuffer()
    {
        Vector2Int moveDirection = Vector2Int.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            moveDirection = Vector2Int.up;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            moveDirection = Vector2Int.down;
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            moveDirection = Vector2Int.left;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            moveDirection = Vector2Int.right;

        if (moveDirection != Vector2Int.zero)
            bufferedDirection = moveDirection;
    }

    public void CollectItem()
    {
        itemsCollected++;
        Debug.Log("아이템 획득 총: " + itemsCollected);
    }

    public void OnCaughtByEnemy()
    {
        Debug.Log("플레이어가 적에게 잡혔습니다");
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }
        else
        {
            Time.timeScale = 0f;
        }
    }
}