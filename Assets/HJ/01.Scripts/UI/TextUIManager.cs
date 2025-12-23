using TMPro;
using UnityEngine;

public class TextUIManager : MonoBehaviour
{
    //ClearUI에는 절대 넣으면 안돼ㅐㅐㅐ
    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private TextMeshProUGUI timerTxt;
    private int stageIdx;

    private void Awake()
    {
        gameObject.SetActive(false);
        stageIdx = StageManager.Instance.CurrentStage;
    }

    private void OnEnable()
    {
        if (GameManager.Instance == null) return;

        levelTxt.text = "스테이지 : "+stageIdx;
        timerTxt.text = GameManager.Instance.Timer.ToString("N2") + "s";
    }
}
