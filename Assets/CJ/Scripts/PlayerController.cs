using System.Collections;
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

        // 1) 좌우 입력이면: flipX 설정 + flipY 무조건 false
        if (LastDirection.x != 0)
        {
            spriteRenderer.flipX = (LastDirection.x < 0); // A: true, D: false
            spriteRenderer.flipY = false;                 // 좌우 이동 시에는 항상 원래대로
            return;
        }

        // 2) 위/아래 입력이면: flipX는 그대로 두고 flipY만 W에서 true, S에서 false
        if (LastDirection.y != 0)
        {
            // flipX는 이전 값 유지 (정면/후면만 뒤집기)
            spriteRenderer.flipY = (LastDirection.y > 0); // W: true, S: false
        }
    }


    public void CollectItem()
    {
        itemsCollected++;
        Debug.Log("아이템 획득! 총: " + itemsCollected);
    }
    public override void Dead()
    {
        // 입력 잠금 (직접 구현한 InputManager 있으면 거기서 막기)
        // InputManager.Instance.Enabled = false;

        StartCoroutine(Co_StylishDeath());
    }

    private IEnumerator Co_StylishDeath()
    {
        // 1) 슬로 모션 + 카메라 쉐이크
        Time.timeScale = 0.2f;
        CameraShaker.Instance.Shake(0.2f, 0.3f); // (진폭, 시간) 직접 구현

        // 2) 플레이어 축소 + 투명도 페이드
        float duration = 0.6f;
        float t = 0f;
        SpriteRenderer sr = spriteRenderer;
        Color startColor = sr.color;
        Vector3 startScale = transform.localScale;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime; // 슬로 모션 영향 안 받게
            float lerp = t / duration;

            // 점점 작게
            transform.localScale = Vector3.Lerp(startScale, startScale * 0.2f, lerp);
            // 투명도 감소
            sr.color = new Color(startColor.r, startColor.g, startColor.b, 1f - lerp);

            yield return null;
        }

        // 3) 화면 블랙 페이드
        yield return ScreenFader.Instance.FadeOutCoroutine(0.8f);

        // 4) 타임 되돌리고 GameOver
        Time.timeScale = 1f;

        if (GameManager.Instance != null)
            GameManager.Instance.GameOver();

        Destroy(gameObject);
    }

}
