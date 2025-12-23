using UnityEngine;

namespace HJ
{
    public class KillObj : MonoBehaviour
    {
        [SerializeField] private Sprite deadImg;
        private SpriteRenderer _spr;

        private void Awake()
        {
            _spr = GetComponent<SpriteRenderer>();
            deadImg ??= _spr.sprite;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //if
            if (collision.TryGetComponent<IKillable>(out IKillable k))
            {
                _spr.sprite = deadImg;
                k.Dead();
            }
        }
    }
}
