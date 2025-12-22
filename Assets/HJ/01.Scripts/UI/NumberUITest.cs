using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class NumberUITest : MonoBehaviour
{
    [SerializeField] private Image one;
    [SerializeField] private Image two;
    [SerializeField] private Image three;
    [SerializeField] private Image go;
    private Sequence sq;

    private void Start()
    {
        Time.timeScale = 0;
        one.transform.localScale = new Vector2(0, 0);
        two.transform.localScale = new Vector2(0, 0);
        three.transform.localScale = new Vector2(0, 0);
        go.transform.localScale = new Vector2(0, 0);

        sq = DOTween.Sequence();
        sq.SetUpdate(true);
        sq.Append(one.rectTransform.DOScale(new Vector2(1,1), 0.5f));
        sq.Append(two.rectTransform.DOScale(new Vector2(1, 1), 0.5f))
            .Join(one.DOFade(0,0.5f));
        sq.Append(three.rectTransform.DOScale(new Vector2(1,1), 0.5f))
            .Join(two.DOFade(0, 0.5f)) ;
        sq.Append(go.rectTransform.DOScale(new Vector2(1,1), 0.6f).SetEase(Ease.OutCubic))
            .Join(three.DOFade(0, 0.5f));
        sq.Append(go.DOFade(0, 0.6f));
        sq.OnComplete(StartGame);

    }

    private void StartGame()
    {
        Time.timeScale = 1;
    }
}
