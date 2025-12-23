using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CircleCollider2D))]
public class StageUI : MonoBehaviour
{
    [SerializeField] private string loadSceneName;
    public bool active;
    [SerializeField] private int stageIdx;

    private void OnMouseDown()
    {
        if(active)
        {
            StageManager.Instance.CurrentStage = stageIdx;
            SceneLoadManager.Instance.LoadScene(loadSceneName);
        }
    }

    private void OnMouseEnter()
    {
        if(active)
            transform.DOScale(new Vector3(1.2f, 1.2f,1f), 0.3f);
    }

    private void OnMouseExit()
    {
        if(active)
            transform.DOScale(new Vector3(1f, 1f,1f), 0.3f);
    }
}
