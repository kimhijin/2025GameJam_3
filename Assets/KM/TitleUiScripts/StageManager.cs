using DG.Tweening;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    static public StageManager Instance;
    public int nowStageNum = 0;
    public List<int> clearStageStarNums = new List<int>();
    public List<float> clearStageTimers = new List<float>();
    [SerializeField] private List<StageUI> StageList = new List<StageUI>();

    private int totalClearStage = 0;
    public int CurrentStage { get; set; } //�ش罺�������� Indx��ȣ�� ���������� ���� ��� ����
    private void Awake()
    {
        if(PlayerPrefs.GetInt("TotalClearStage", -1) == -1)
        {
            ClearAllData();
        }
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
        bool active = arg0.name == "Stage";

        for (int i = 0; i < StageList.Count; i++)
        {
            StageList[i].gameObject.SetActive(active);
            if (active)
            {
                StageList[i].GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1f);
                StageList[i].active = false;
                Debug.Log(StageList[i].enabled);
            }
        }
        LoadData();
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
        if(clearStageStarNums.Count + 1 > nowStageNum)
        {
            clearStageStarNums[CurrentStage - 1] = data.startCnt;
            clearStageTimers[CurrentStage - 1] = data.timer;
        }
        else
        {
            clearStageStarNums.Add(data.startCnt);
            clearStageTimers.Add(data.timer);
        }

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
        nowStageNum = PlayerPrefs.GetInt("NowStageNum_", 1);
        totalClearStage = PlayerPrefs.GetInt("TotalClearStage", 0);
        for (int i = 0; i < totalClearStage; i++)
        {
            int starNum = PlayerPrefs.GetInt($"ClearStageStarNums_{i}", 0);
            float timerCount = PlayerPrefs.GetFloat($"ClearStageTimerCount_{i}", int.MaxValue);
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
                if(clearStageTimers[i] == int.MaxValue)
                {
                    timerparent.GetComponent<TMP_Text>().text = " ";
                    continue;
                }
                timerparent.GetComponent<TMP_Text>().text = clearStageTimers[i].ToString("F2") + "s";
            }
        }
    }

    [ContextMenu("ClearAllData")]
    public void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("TotalClearStage", 1);
        nowStageNum = 1;
        clearStageStarNums.Clear();
        clearStageTimers.Clear();
        SaveData();
    }

    [ContextMenu("AddStageNum")]
    public void AddStageNum()
    {
        if(CurrentStage + 1 > nowStageNum)
        {
            nowStageNum = CurrentStage + 1;
        }
    }
    
    
    #region  Never Use This KHJ.
    [ContextMenu("AddStageNumTest")]
    public void TestCurrentStageNum()
    {
        CurrentStage += 1;
        Debug.Log(CurrentStage);
    }

    [ContextMenu("MinusStageNumTest")]
    public void MinusStageNumTest()
    {
        CurrentStage -= 1;
        Debug.Log(CurrentStage);
    }
    #endregion

    [ContextMenu("SaveData")]
    public void SaveData()
    {
        Data data = new Data();
        data.startCnt = 0;
        data.timer = int.MaxValue;
        SaveStage(data);
    }
}