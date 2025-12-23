using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClearUIAnimation : MonoBehaviour
{
    [Header("���������� �ð������� ��")]
    [SerializeField] private float twoTime; //�� �ΰ� �ִ�ð�
    [SerializeField] private float threeTime; //�� ���� �ִ�ð�
    [SerializeField] private GameObject starObj;
    [SerializeField] private RectTransform starParent;
    private float currentTime;

    [Header("��ư")]
    [SerializeField] private Button[] btns; //���� ��� 

    [SerializeField] private Image goodImg;

    [Header("���")]
    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private TextMeshProUGUI timerTxt;

    [SerializeField] private int stageIdx;

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        Debug.Log("Open ClearUI");
        currentTime = GameManager.Instance.Timer;
        SpawnStar();
        Time.timeScale = 0;
    }

    private void Init()
    {
        StageManager.Instance.CurrentStage = stageIdx;
        foreach(var item in btns)
        {
            item.enabled = false;
            Color cor = item.image.color;
            cor.a = 0;
            item.image.color = cor;
        }

        goodImg.color = new Color(1, 1, 1, 0);
        goodImg.transform.localScale = new Vector3(10, 10, 1);
        gameObject.SetActive(false);
    }

    private void SpawnStar()
    {
        int starCnt = 1;

        if(currentTime <=threeTime)
            starCnt = 3;
        else if(currentTime <= twoTime)
            starCnt = 2;

        for(int i =0; i<starCnt; ++i)
        {
            GameObject star= Instantiate(starObj, starParent);
            star.transform.localScale = new Vector2(15, 15);
            star.transform.DOScale(1, 0.7f).SetUpdate(true).SetEase(Ease.InSine)
                .OnComplete(()=>
                { 
                    StartCoroutine(ContinueRotation(star,false));  
                    ShowBtn();
                    ShowGoodPng();
                });
        }

        SaveData(starCnt);
    }

    private IEnumerator ContinueRotation(GameObject rotationObj, bool right)
    {
        float t = 0;
        while(true)
        {
            t += Time.unscaledDeltaTime/2;
            t = t % 1;
            float angle;
            if (right)
                angle = Mathf.Lerp(0f, 360f, t);
            else
                angle = Mathf.Lerp(360f, 0f, t);

                rotationObj.transform.localRotation = Quaternion.Euler(0, 0, angle);
            yield return null;
        }
    }

    private void ShowGoodPng()
    {
        goodImg.DOFade(1, 0.2f).SetUpdate(true);
        goodImg.transform.DOScale(new Vector3(1, 1, 1), 0.5f).SetEase(Ease.InSine).SetUpdate(true)
            .OnComplete(()=>StartCoroutine(ContinueRotation(goodImg.gameObject, true)));
    }

    private void ShowBtn()
    {
        foreach(var item in btns)
        {
            item.image.DOFade(1, 0.7f).SetUpdate(true)
                .OnComplete(()=>item.enabled = true);
        }
    }

    private void SaveData(int starCnt)
    {
        StageManager.Instance.CurrentStage = stageIdx;
        Debug.LogWarning(stageIdx - 1);
        Debug.Assert(StageManager.Instance.clearStageTimers[stageIdx-1] != null, "Sex");
        float timer = StageManager.Instance.clearStageTimers[stageIdx-1];
        Debug.Log("Current Timer " + timer);

        Data data = new Data();
        if (StageManager.Instance.clearStageStarNums[stageIdx-1] <= starCnt)
            data.startCnt = starCnt;
        if (timer >= currentTime)
            timer = currentTime;
        Debug.Log("Timer "+timer);
        data.timer = timer;

        levelTxt.text = "Stage : " + (stageIdx);
        timerTxt.text = timer.ToString("N2") + "s"; 

        StageManager.Instance.SaveStage(data);  
        StageManager.Instance.AddStageNum();

        Debug.Log(StageManager.Instance.CurrentStage);
        if(StageManager.Instance.CurrentStage == 5 || StageManager.Instance.nowStageNum > stageIdx + 1)
        {
            return;
        }
        Data data2 = new Data();
        data2.startCnt = 0;
        data2.timer = int.MaxValue;
        StageManager.Instance.SaveStage(data2);
    }

}
