using System;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    private bool isLocked = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isLocked) return;
        if (KeyManager.Instance.UseKey())
        {
            isLocked = false;
            Destroy(gameObject);
        }
    }
}