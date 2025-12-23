using TMPro;
using UnityEngine;

public class TextUIManager : MonoBehaviour
{
    //ClearUI���� ���� ������ �ȵŤ�����
    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private TextMeshProUGUI timerTxt;
    private int stageIdx;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (GameManager.Instance == null) return;
        stageIdx = StageManager.Instance.CurrentStage;

        levelTxt.text = "Stage : "+stageIdx;
        timerTxt.text = GameManager.Instance.Timer.ToString("N2") + "s";
    }
}
