using TMPro;
using UnityEngine;

public class TextUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private string levelName;

    [SerializeField] private TextMeshProUGUI timerTxt;

    [SerializeField] private bool isActive;

    private void Awake()
    {
        if(!isActive)
            gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (GameManager.Instance == null) return;
        levelTxt.text = levelName;
        Debug.Assert(timerTxt != null, " timer Txt");
        Debug.Assert(GameManager.Instance != null, " GameManager is null");
        timerTxt.text = GameManager.Instance.Timer.ToString("N2") + "s";
    }
}
