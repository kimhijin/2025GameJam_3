using System.Collections;
using UnityEngine;

public class HoleTrap : MonoBehaviour
{
    [SerializeField] private Sprite openImg;
    [SerializeField] private Sprite closeImg;
    private SpriteRenderer _spr;

    [SerializeField] private BtnObject[] openBtns;
    [SerializeField] private BtnObject[] closeBtns;
    [SerializeField] private Vector2 boxSize;
    private bool canKill;
    private int count = 0;

    private void Awake()
    {
        _spr = GetComponent<SpriteRenderer>();

        foreach(var item in openBtns)
        {
            item.OnEnter += HandleOpenHole;
            item.OnExit += HandleCloseHole;
        }

        foreach (var item in closeBtns)
        {
            item.OnEnter += CBTHandleCloseHole;
            item.OnExit += CBTHandleOpenHole;
        }
    }

    private void OnDestroy()
    {

        foreach (var item in openBtns)
        {
            item.OnEnter += HandleOpenHole;
            item.OnExit += HandleCloseHole;
        }

        foreach (var item in closeBtns)
        {
            item.OnEnter -= CBTHandleCloseHole;
            item.OnExit -= CBTHandleOpenHole;
        }
    }

    private void CBTHandleOpenHole()
    {
        ++count;
        if (count >= openBtns.Length)
        {
            _spr.sprite = openImg;
            gameObject.layer = LayerMask.NameToLayer("Obstacle");
            canKill = true;
        }
    }

    private void CBTHandleCloseHole()
    {
        _spr.sprite = closeImg;
        gameObject.layer = LayerMask.NameToLayer("Default");
        --count;
        canKill = false;
    }

    private void HandleOpenHole()
    {
        ++count;
        if(count >= openBtns.Length)
        {
            _spr.sprite = openImg;
            gameObject.layer = LayerMask.NameToLayer("Obstacle");
            canKill = true;
        }
    }

    private void HandleCloseHole()
    {
        _spr.sprite = closeImg; 
        gameObject.layer = LayerMask.NameToLayer("Default");
        --count;
        canKill = false;
    }

    private void FixedUpdate()
    {
        if(canKill)
        {
            Collider2D asibar = Physics2D.OverlapBox(transform.position, boxSize,0f, LayerMask.GetMask("Enemy", "Player"));

            if(asibar != null)
            {
                if (asibar.TryGetComponent<IKillable>(out IKillable k))
                {
                    k.Dead();
                }
            }
        }
        else
        {
            return;   
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}
