using System.Collections;
using UnityEngine;

public class PlayerController : Agent
{
    [SerializeField] private float moveInterval = 0.1f;
    [SerializeField] private AnimatorOverrideController playerAnimatorOverride;

    private float moveTimer = 0f;
    private Vector2Int bufferedDirection = Vector2Int.zero;
    public Vector2Int LastDirection { get; private set; } = Vector2Int.right;
    
    private bool isDead = false;

    public int itemsCollected { get; private set; } = 0;

    private SpriteRenderer spriteRenderer;
    private bool wasMovingLastFrame = false;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        LastDirection = Vector2Int.right;

        if (animator != null && playerAnimatorOverride != null)
        {
            animator.runtimeAnimatorController = playerAnimatorOverride;
        }
    }

    protected override void Update()
    {
        base.Update();
        if (isDead) return;

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

        if (isMoving != wasMovingLastFrame)
        {
            UpdateAnimatorParameters();
            wasMovingLastFrame = isMoving;
        }
    }

    private void ReadInputToBuffer()
    {
        if (isMoving || isDead) return;

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
        animator.SetBool("IsMoving", isMoving);

        bool isUpDown = (LastDirection.y != 0);
        animator.SetBool("IsRight", !isUpDown);
    }

    private void UpdateFlip()
    {
        if (spriteRenderer == null) return;

        if (LastDirection.x != 0)
        {
            spriteRenderer.flipX = (LastDirection.x < 0);
            spriteRenderer.flipY = false;             
            return;
        }

        if (LastDirection.y != 0)
        {
            spriteRenderer.flipY = (LastDirection.y > 0);
        }
    }

    public override void Dead()
    {
        isDead = true;
        StartCoroutine(Co_StylishDeath());
    }

    private IEnumerator Co_StylishDeath()
    {
        Time.timeScale = 0.2f;
        CameraShaker.Instance.Shake(0.2f, 0.3f);

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
        yield return ScreenFader.Instance.FadeOutCoroutine(0.8f);

        Time.timeScale = 1f;

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;
        if (collision.GetComponent<IKillable>() != null)
        {
            Dead();
        }
        if(collision.TryGetComponent<IItem>(out IItem item))
        {
            item.GetItem();
        }
    }
}
