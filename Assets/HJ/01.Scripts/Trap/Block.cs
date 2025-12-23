using UnityEngine;

namespace HJ
{
    [RequireComponent(typeof(BoxCollider2D),typeof(Rigidbody2D))]
    public class Block : MonoBehaviour
    {
        [SerializeField] private Sprite openImg;
        [SerializeField] private Sprite closeImg;
        private SpriteRenderer _spr;
        private Collider2D _col;

        [SerializeField] private BtnObject[] btnObj;
        private int count = 0;

        [SerializeField] private bool isActive;

        private void Awake()
        {
            _spr = GetComponent<SpriteRenderer>();
            _col = GetComponent<Collider2D>();

            _col.enabled = true;

            foreach (var item in btnObj)
            {
                item.OnEnter += HandleOpenHole;
            }
        }

        private void OnDestroy()
        {
            foreach (var item in btnObj)
            {
                item.OnEnter -= HandleOpenHole;
            }
        }

        private void HandleOpenHole()
        {
            ++count;
            if (count >= btnObj.Length)
            {
                isActive = !isActive;
                

                _spr.sprite = openImg;
                _col.enabled = false;   
            }
        }
    }
}
