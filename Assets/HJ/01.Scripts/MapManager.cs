using HJ;
using System;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Fruit[] fruit;
    private int fruitCnt;
    public event Action OnClear;
    public event Action OnDead;

    public static MapManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void HandleDead()
    {
        OnDead?.Invoke();
    }

    public void CheckFruit()
    {
        ++fruitCnt;
        if(fruitCnt >= fruit.Length)
        {
            OnClear?.Invoke();
        }
    }
}
