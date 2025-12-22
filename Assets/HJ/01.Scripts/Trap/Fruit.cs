using UnityEngine;

namespace HJ
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Fruit : MonoBehaviour, IItem
    {
        [SerializeField] private Sprite eatImg;
        private SpriteRenderer _spr;
        private Collider2D _col;

        private void Awake()
        {
            _col = GetComponent<Collider2D>();
            _spr = GetComponent<SpriteRenderer>();
        }

        public void GetItem()
        {
            Debug.Log("GetItme " + gameObject.name);
            MapManager.Instance.CheckFruit();
            _spr.sprite = eatImg;
            _col.enabled = false;
        }
    }
}
