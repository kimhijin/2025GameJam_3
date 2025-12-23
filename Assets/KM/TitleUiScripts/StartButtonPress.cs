using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonPress : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    public void OnStartButtonClicked()
    {
        GetComponent<AudioSource>().Play();
        SceneChangeResner.Instance.Change();
        DOTween.To(x => audioSource.volume = x, audioSource.volume, 0f, 2f).OnComplete(() =>
        {
            SceneManager.LoadScene("Stage");
        });
    }
}
