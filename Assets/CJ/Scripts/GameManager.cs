using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool gameOverFlag = false;

    public GameOverUI _overUI;

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
        _overUI.gameObject.SetActive(true);
    }
}