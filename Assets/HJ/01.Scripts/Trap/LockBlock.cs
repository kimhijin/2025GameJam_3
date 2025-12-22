using UnityEngine;

namespace HJ
{
    [RequireComponent(typeof(BoxCollider2D),typeof(Rigidbody2D))]
    public class LockBlock : MonoBehaviour
    {


        private void Awake()
        {
            ItemManager.Instance.OnGetKey += HandleOpenDoor;
        }

        private void OnDestroy()
        {
            ItemManager.Instance.OnGetKey -= HandleOpenDoor;
        }

        private void HandleOpenDoor()
        {
            
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
               
        }
    }
}
