using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool gameOverFlag = false;

    [SerializeField] private GameOverUI _overUI;
    public float Timer { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        Timer = 0;
    }

    private void Update()
    {
        Timer += Time.deltaTime;
    }

    public void GameOver()
    {
        if (gameOverFlag)
            return;

        gameOverFlag = true;
        _overUI.gameObject.SetActive(true);
    }
}