using UnityEngine;

public class TestEnabled : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log("켜질게요;;");
    }

    private void OnDisable()
    {
        Debug.Log("isClear");
        if(MapManager.Instance.isClear)
        {
            gameObject.SetActive(true);
            Debug.Log("활성화 ㅇㅇ");
        }
    }
}
