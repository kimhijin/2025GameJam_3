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
        ++count;
        if(count >= btnObj.Length)
        {
            _spr.sprite = openImg;
            canKill = true;
        }
    }

    private void HandleCloseHole()
    {
        _spr.sprite = closeImg;
        --count;
        canKill = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(canKill)
        {
            //어떤 Agent이든지 죽이기
            if(collision.TryGetComponent<IKillable>(out IKillable k))
            {
                k.Dead();
            }
        }
    }
}
