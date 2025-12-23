using System.Collections;
using UnityEngine;

public class RemoveButton : MonoBehaviour
{
    public void RemoveData()
    {
        StageManager.Instance.ClearAllData();
        StartCoroutine(WaitForSaving());
    }

    private IEnumerator WaitForSaving()
    {
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }
}
