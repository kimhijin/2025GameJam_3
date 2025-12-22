using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class BtnObject : MonoBehaviour
{
    [SerializeField] private Sprite enterImg;
    [SerializeField] private Sprite exitImg;
    //들어오면 신호주는 오브젝트
    public event Action OnEnter;
    public event Action OnExit;
    private SpriteRenderer _spr;

    private void Awake()
    {
        _spr = GetComponent<SpriteRenderer>();

        exitImg ??= _spr.sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Agent인지 계속 확인
        OnEnter?.Invoke();
        if(enterImg != null)
            _spr.sprite = enterImg;
        //이미지 변환
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Agent인지 계속 확인
        OnExit?.Invoke();
        if(exitImg != null)
            _spr.sprite = exitImg;
        // 이미지 변환
    }
}
