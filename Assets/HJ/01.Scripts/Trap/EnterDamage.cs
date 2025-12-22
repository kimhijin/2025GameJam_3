using UnityEngine;

public class EnterDamage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if
        if(collision.TryGetComponent<IKillable>(out IKillable k))
        {
            k.Dead();
        }
    }
}
