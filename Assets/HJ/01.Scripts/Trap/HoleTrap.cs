using System.Collections;
using UnityEngine;

public class HoleTrap : MonoBehaviour
{
    [SerializeField] private Sprite openImg;
    [SerializeField] private Sprite closeImg;
    private SpriteRenderer _spr;

    [SerializeField] private BtnObject[] openBtns;
    [SerializeField] private BtnObject[] closeBtns;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(canKill)
        {
            //� Agent�̵��� ���̱�
            if(collision.TryGetComponent<IKillable>(out IKillable k))
            {
                k.Dead();
            }
        }
    }
}
