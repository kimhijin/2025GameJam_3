using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(new Vector2(1.15f, 1.15f), 0.3f).SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(new Vector2(1f, 1f), 0.3f).SetUpdate(true);
    }
}
