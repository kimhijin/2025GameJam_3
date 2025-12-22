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
    [SerializeField] private Button[] btns; //순서 상관 ㄴㄴ

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        //currentTime = GameManager.Instance.Timer;
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
                    StartCoroutine(ContinueRotation(star));  
                    ShowBtn(); 
                });
        }
    }

    private IEnumerator ContinueRotation(GameObject rotationObj)
    {
        float t = 0;
        while(true)
        {
            t += Time.unscaledDeltaTime/2;
            t = t % 1;
            float angle = Mathf.Lerp(0f, 360f, t);

            rotationObj.transform.localRotation = Quaternion.Euler(0, 0, angle);
            yield return null;
        }
    }

    private void ShowBtn()
    {
        foreach(var item in btns)
        {
            item.image.DOFade(1, 0.7f).SetUpdate(true)
                .OnComplete(()=>item.enabled = true);
        }
    }

    private void TimeUI()
    {

    }    
}
