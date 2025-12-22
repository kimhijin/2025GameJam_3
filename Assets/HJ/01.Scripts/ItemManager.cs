using System;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public event Action OnGetKey;

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
}
