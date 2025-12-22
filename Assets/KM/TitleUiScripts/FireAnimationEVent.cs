using UnityEngine;

public class FireAnimationEVent : MonoBehaviour
{
    [SerializeField] private RunAwayWheneE runAwayWheneE;
    public void OnnnnnStartAnimationEnded()
    {
        Debug.Log("IsActiveTrue");
        runAwayWheneE.gameObject.transform.parent.gameObject.SetActive(true);
        runAwayWheneE.Exit();
    }
}
