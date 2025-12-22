using System.Collections;
using UnityEngine;

public class HoleTrap : MonoBehaviour
{
    [SerializeField] private Sprite openImg;
    [SerializeField] private Sprite closeImg;
    private SpriteRenderer _spr;

    [SerializeField] private BtnObject[] btnObj;
    private bool canKill;
    private int count = 0;

    private void Awake()
    {
        _spr = GetComponent<SpriteRenderer>();

        foreach(var item in btnObj)
        {
            item.OnEnter += HandleOpenHole;
            item.OnExit += HandleCloseHole;
        }
    }

    private void OnDestroy()
    {
        foreach (var item in btnObj)
        {
            item.OnEnter -= HandleOpenHole;
            item.OnExit -= HandleCloseHole;
        }
    }

    private void HandleOpenHole()
    {
        Debug.Log("HoleEnter");
        ++count;
        if(count >= btnObj.Length)
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
