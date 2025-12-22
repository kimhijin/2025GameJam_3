using HJ;
using System;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private Fruit[] fruit;
    private int fruitCnt;
    public event Action OnGetKey;
    public event Action OnClear;

    public static ItemManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void CheckKey()
    {
        OnGetKey?.Invoke();
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
