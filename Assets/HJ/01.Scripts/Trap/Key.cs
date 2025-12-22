using UnityEngine;

namespace HJ
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
    public class Key : MonoBehaviour
    {
        //Physics 건들여서 플레이어랑 적만 닿게 하기

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //트랩 매니저에 신호 보내기
            ItemManager.Instance.CheckKey();
            Destroy(gameObject);
        }
    }
}
