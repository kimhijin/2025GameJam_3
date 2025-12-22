using UnityEngine;

public class HJ_TestPlayer : MonoBehaviour, IKillable
{
    public void Dead()
    {
        Debug.Log("±×´Â Á×¾ú´Ù");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<IItem>(out IItem item))
        {
            item.GetItem();
        }
    }
}
