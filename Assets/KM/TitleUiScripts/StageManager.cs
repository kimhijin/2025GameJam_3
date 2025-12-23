using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    static public StageManager Instance;
    [SerializeField] private int nowStageNum = 0;
    [SerializeField] private List<int> clearStageStarNums = new List<int>();
    [SerializeField] private List<float> clearStageTimers = new List<float>();
    [SerializeField] private List<StageUI> StageList = new List<StageUI>();

    private int totalClearStage = 0;
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

        SceneManager.sceneLoaded += StageActive;
    }

    private void StageActive(Scene arg0, LoadSceneMode arg1)
    {
        bool active= arg0.name == "Stage";
        foreach(var item in StageList)
        {
            item.gameObject.SetActive(active);
        }
    }

    private void Start()
    {
        for(int i = 0; i < StageList.Count; i++)
        {
            StageList[i].GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1f);
            StageList[i].active = false;
            Debug.Log(StageList[i].enabled);
        }
        LoadData();
    }

    

    public void SaveStage(Data data)
    {
        clearStageStarNums.Add(data.startCnt);
        clearStageTimers.Add(data.timer);
        PlayerPrefs.SetInt("TotalClearStage", clearStageStarNums.Count);
        PlayerPrefs.SetInt("NowStageNum_", nowStageNum);
        totalClearStage = clearStageStarNums.Count;

        for(int i = 0; i < totalClearStage; i++)
        {
            PlayerPrefs.SetInt($"ClearStageStarNums_{i}", clearStageStarNums[i]);
            PlayerPrefs.SetFloat($"ClearStageTimerCount_{i}", clearStageTimers[i]);
        }
    }

    [ContextMenu ("LoadData")]
    public void LoadData()
    {
        clearStageStarNums.Clear();
        clearStageTimers.Clear();
        nowStageNum = PlayerPrefs.GetInt("NowStageNum_", 0);
        totalClearStage = PlayerPrefs.GetInt("TotalClearStage", 0);
        for (int i = 0; i < totalClearStage; i++)
        {
            int starNum = PlayerPrefs.GetInt($"ClearStageStarNums_{i}", 0);
            float timerCount = PlayerPrefs.GetFloat($"ClearStageTimerCount_{i}", 0f);
            clearStageStarNums.Add(starNum);
            clearStageTimers.Add(timerCount);
            Debug.Log($"Loaded Stage {i}: Stars = {starNum}, Timer = {timerCount}");
        }

        for(int i = 0; i < StageList.Count; i++)
        {
            if(i < nowStageNum)
            {
                StageList[i].active = true;
                StageList[i].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                GameObject starparent = StageList[i].transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
                GameObject timerparent = StageList[i].transform.GetChild(1).gameObject.transform.GetChild(1).gameObject;
                for(int j = 0; j < clearStageStarNums[i]; j++)
                {
                    starparent.transform.GetChild(j).gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                }
                timerparent.GetComponent<TMP_Text>().text = clearStageTimers[i].ToString("F2") + "s";
            }
        }
    }

    [ContextMenu("ClearAllData")]
    public void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
        nowStageNum = 0;
        clearStageStarNums.Clear();
        clearStageTimers.Clear();
    }

    [ContextMenu("AddStageNum")]
    public void AddStageNum()
    {
        nowStageNum++;
    }

    [ContextMenu("SaveData")]
    public void SaveData()
    {
        Data data = new Data();
        data.startCnt = Random.Range(1,3);
        data.timer = Random.Range(10f,20f);
        SaveStage(data);
    }
}