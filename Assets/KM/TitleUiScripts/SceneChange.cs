using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void Changed()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
