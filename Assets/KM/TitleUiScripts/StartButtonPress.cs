using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonPress : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        SoundManager.Instance.PlaySFX("Click");
        SceneLoadManager.Instance.LoadScene("Stage");
    }
}
