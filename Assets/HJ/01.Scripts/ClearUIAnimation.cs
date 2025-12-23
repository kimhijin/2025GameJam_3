using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ClearUIAnimation : MonoBehaviour
{
    [Header("스테이지별 시간에따른 별")]
    [SerializeField] private float twoTime; //별 두개 최대시간
    [SerializeField] private float threeTime; //별 세개 최대시간
    [SerializeField] private GameObject starObj;
    [SerializeField] private RectTransform starParent;
    private float currentTime;

    [Header("버튼")]
    [SerializeField] private Button[] btns; //순서 상관 

    [SerializeField] private Image goodImg;

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        currentTime = GameManager.Instance.Timer;
        SpawnStar();
        Time.timeScale = 0;
    }

    private void Init()
    {
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
        goodImg.DOFade(1, 0.2f);
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
        Data data = new Data();
        data.startCnt = starCnt;
        data.timer = currentTime;

        StageManager.Instance.AddStageNum();
        StageManager.Instance.SaveStage(data);  
    }
}
