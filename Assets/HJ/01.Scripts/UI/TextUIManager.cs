using TMPro;
using UnityEngine;

public class TextUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private string levelName;

    [SerializeField] private TextMeshProUGUI timerTxt;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        levelTxt.text = levelName;
        Debug.Assert(timerTxt != null, " timer Txt");
        Debug.Assert(GameManager.Instance != null, " GameManager is null");
        timerTxt.text = GameManager.Instance.Timer.ToString("N2") + "s";
    }
}
