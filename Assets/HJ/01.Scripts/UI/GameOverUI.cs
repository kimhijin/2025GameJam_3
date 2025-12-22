using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Image agentImg;
    [SerializeField] private Image backgrountImg;


    private void Awake()
    {
        Color col = Color.red;
        col.a = 0;
        backgrountImg.color = col;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        gameObject.SetActive(true);

        agentImg.transform.localScale = new Vector2(1, 1);
        agentImg.transform.DOScale(new Vector2(10, 10), 1);

        backgrountImg.DOFade(1, 1f)
            .OnComplete(()=>SceneManager.LoadScene(SceneManager.GetActiveScene().name));
    }
}
