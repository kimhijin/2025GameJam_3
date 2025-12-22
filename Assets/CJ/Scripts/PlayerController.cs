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

        // IsMoving
        animator.SetBool("IsMoving", isMoving);

        // IsRight: 좌우면 true, 위아래면 false
        bool isUpDown = (LastDirection.y != 0);
        animator.SetBool("IsRight", !isUpDown);
    }

    private void UpdateFlip()
    {
        if (spriteRenderer == null) return;

        // 좌우 플립은 항상 처리
        spriteRenderer.flipX = (LastDirection.x < 0);

        // IsRight가 false(위/아래)일 때만 flipY 적용
        bool isUpDown = (LastDirection.y != 0);
        if (!isUpDown)   // IsRight = true 인 상황(좌우)이면 flipY 건들지 않음
            return;

        // 위/아래일 때만 flipY
        spriteRenderer.flipY = (LastDirection.y > 0);
    }

    public void CollectItem()
    {
        itemsCollected++;
        Debug.Log("아이템 획득! 총: " + itemsCollected);
    }
    public override void Dead()
    {
        base.Dead();
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }
        Destroy(gameObject);
    }
}
