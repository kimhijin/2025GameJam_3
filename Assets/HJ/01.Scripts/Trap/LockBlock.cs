using System.Net.WebSockets;
using UnityEngine;

namespace HJ
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class LockBlock : MonoBehaviour
    {
        [SerializeField] private Sprite openImg;
        [SerializeField] private Sprite closeImg;
        private SpriteRenderer _spr;

        [SerializeField] private BtnObject[] openBtns;
        [SerializeField] private BtnObject[] closeBtns;
        private int count = 0;

        private void Awake()
        {
            _spr = GetComponent<SpriteRenderer>();
            _spr.sprite = closeImg;

            foreach (var item in openBtns)
            {
                item.OnEnter += HandleOpenHole;
                item.OnExit += HandleCloseHole;
            }

            foreach (var item in closeBtns)
            {
                item.OnEnter += CBTHandleCloseHole;
                item.OnExit += CBTHandleOpenHole;
            }
        }

        private void OnDestroy()
        {

            foreach (var item in openBtns)
            {
                item.OnEnter += HandleOpenHole;
                item.OnExit += HandleCloseHole;
            }

            foreach (var item in closeBtns)
            {
                item.OnEnter -= CBTHandleCloseHole;
                item.OnExit -= CBTHandleOpenHole;
            }
        }

        private void CBTHandleOpenHole()
        {
            if (count >= openBtns.Length)
            {
                _spr.sprite = openImg;
                gameObject.layer = LayerMask.NameToLayer("Default");
            }
            ++count;

        }

        private void CBTHandleCloseHole()
        {
            --count;
            _spr.sprite = closeImg;
            gameObject.layer = LayerMask.NameToLayer("Obstacle");
        }

        private void HandleOpenHole()
        {
            ++count;
            if (count >= openBtns.Length)
            {
                _spr.sprite = openImg;
                gameObject.layer = LayerMask.NameToLayer("Default");
            }

        }

        private void HandleCloseHole()
        {
            --count;
            _spr.sprite = closeImg;
            gameObject.layer = LayerMask.NameToLayer("Obstacle");
        }
    }
}
