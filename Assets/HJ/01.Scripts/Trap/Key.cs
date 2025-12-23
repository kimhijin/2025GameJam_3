using System;
using UnityEngine;

public class Key : MonoBehaviour, IItem
{
    private bool IsGet = false;
    public void GetItem()
    {
        if (IsGet) return;
        KeyManager.Instance.AddKey(1);
        IsGet = true;
        Destroy(gameObject);
    }
}