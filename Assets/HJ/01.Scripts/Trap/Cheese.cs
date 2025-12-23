using UnityEngine;

namespace HJ
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Cheese : MonoBehaviour, IItem
    {
        [SerializeField] private Sprite eatImg;
        private Collider2D _col;
        private Animator _ani;

        private void Awake()
        {
            _col = GetComponent<Collider2D>();
            _ani = GetComponent<Animator>();
        }

        public void GetItem()
        {
            Debug.Log("GetItme " + gameObject.name);
            SoundManager.Instance.PlaySFX("Cheese");
            MapManager.Instance.CheckFruit();
            _ani.SetTrigger("Eat");
            gameObject.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
            _col.enabled = false;
        }
    }
}
