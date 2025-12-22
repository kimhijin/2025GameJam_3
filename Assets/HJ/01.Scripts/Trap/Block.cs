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
        private bool canKill;
        private int count = 0;

        private void Awake()
        {
            _spr = GetComponent<SpriteRenderer>();
            _col = GetComponent<Collider2D>();

            _col.enabled = true;

            foreach (var item in btnObj)
            {
                item.OnEnter += HandleOpenHole;
                item.OnExit += HandleCloseHole;
            }
        }

        private void OnDestroy()
        {
            foreach (var item in btnObj)
            {
                item.OnEnter -= HandleOpenHole;
                item.OnExit -= HandleCloseHole;
            }
        }

        private void HandleOpenHole()
        {
            Debug.Log("HoleEnter");
            ++count;
            if (count >= btnObj.Length)
            {
                _spr.sprite = openImg;
                canKill = true;

                _col.enabled = false;
            }
        }

        private void HandleCloseHole()
        {
            _spr.sprite = closeImg;
            --count;
            canKill = false;

            _col.enabled = true;
        }
    }
}
