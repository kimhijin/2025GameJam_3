using DG.Tweening;
using UnityEngine;

public class RunAwayWheneE : MonoBehaviour
{
    [SerializeField] RectTransform ExitBtn;
    [SerializeField] Vector2 moveTarget;
    private Vector2 nowPos;
    private bool isFirst = true;
    private void Start()
    {
        nowPos = ExitBtn.anchoredPosition;
        Enter();
    }
    public void Enter()
    {
        if (isFirst)
            {
                isFirst = false;
                gameObject.transform.parent.gameObject.SetActive(false);
                ExitBtn.anchoredPosition = moveTarget;
                return;
            }
        Sequence seq = DOTween.Sequence();
        seq.Append(ExitBtn.DOAnchorPos(moveTarget, 0.3f).SetEase(Ease.OutQuad));
    }

    public void Exit()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(ExitBtn.DOAnchorPos(nowPos, 0.3f).SetEase(Ease.OutQuad));
    }
}
