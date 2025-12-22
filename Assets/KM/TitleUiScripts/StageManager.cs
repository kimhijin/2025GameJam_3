using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    static public StageManager Instance;
    [SerializeField] private int nowStageNum = 0;
    [SerializeField] private List<int> clearStageStarNums = new List<int>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveStage()
    {
        
    }
}
