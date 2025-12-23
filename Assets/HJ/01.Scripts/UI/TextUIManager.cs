using TMPro;
using UnityEngine;

public class TextUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private TextMeshProUGUI timerTxt;

    [SerializeField] private bool isClearUI;
    private int stageIdx;

    private void Awake()
    {
        if(gameObject.activeSelf)
            gameObject.SetActive(false);
        stageIdx = StageManager.Instance.CurrentStage;
    }

    private void OnEnable()
    {
        if (GameManager.Instance == null) return;

        levelTxt.text = "스테이지 : "+stageIdx;
        //timerTxt.text = isClearUI ? GameManager.Instance.Timer.ToString("N2") + "s";
    }
}
