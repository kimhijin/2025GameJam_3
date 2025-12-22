using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool gameOverFlag = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void GameOver()
    {
        if (gameOverFlag)
            return;

        gameOverFlag = true;
        //game over 처리(UI 띄어주고, 씬 재로드 정도?)
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        gameOverFlag = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}