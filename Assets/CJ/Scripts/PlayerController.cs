using UnityEngine;

public class PlayerController : Agent
{
    [SerializeField] private float moveInterval = 0.2f;
    [SerializeField] private AnimatorOverrideController playerAnimatorOverride;

    private float moveTimer = 0f;
    private Vector2Int bufferedDirection = Vector2Int.zero;
    public Vector2Int LastDirection { get; private set; } = Vector2Int.right;

    public int itemsCollected { get; private set; } = 0;

    private SpriteRenderer spriteRenderer;
    private bool wasMovingLastFrame = false;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        LastDirection = Vector2Int.right;

        // Animator Override 설정
        if (animator != null && playerAnimatorOverride != null)
        {
            animator.runtimeAnimatorController = playerAnimatorOverride;
        }
    }

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
                {
                    LastDirection = bufferedDirection;
                    UpdateFlip();
                    TryMove(bufferedDirection);
                }

                bufferedDirection = Vector2Int.zero;
                moveTimer = 0f;
            }
        }

        // IsMoving 상태 변화 감지
        if (isMoving != wasMovingLastFrame)
        {
            UpdateAnimatorParameters();
            wasMovingLastFrame = isMoving;
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

    private void UpdateAnimatorParameters()
    {
        if (animator == null) return;

        // IsMoving 파라미터 업데이트
        animator.SetBool("IsMoving", isMoving);

        // IsRight 파라미터 업데이트
        // 위/아래만 false, 나머지(좌/우)는 true
        bool isUpDown = (LastDirection.y != 0);
        animator.SetBool("IsRight", !isUpDown);
    }

    private void UpdateFlip()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = (LastDirection.x < 0);
            spriteRenderer.flipY = (LastDirection.y > 0);  // 반대로!
        }
    }

    public void CollectItem()
    {
        itemsCollected++;
        Debug.Log("아이템 획득! 총: " + itemsCollected);
    }

    public void OnDrowning()
    {
        Debug.Log("플레이어가 물에 빠져 죽었습니다!");
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }
        else
        {
            Time.timeScale = 0f;
        }
    }

    public void OnCaughtByEnemy()
    {
        Debug.Log("플레이어가 적에게 잡혔습니다!");
        
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
