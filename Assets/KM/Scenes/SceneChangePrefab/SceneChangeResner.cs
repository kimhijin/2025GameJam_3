using UnityEngine;

public class SceneChangeResner : MonoBehaviour
{
    public static SceneChangeResner Instance;

    private Animator _animator;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        _animator = GetComponent<Animator>();
    }

    public void Change()
    {
        _animator.SetBool("Change",true);
    }
}
