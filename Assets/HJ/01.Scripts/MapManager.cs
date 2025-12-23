using HJ;
using System;
using UnityEngine;

[System.Serializable]
public class Data
{
    public int stageNum;
    public int startCnt;
    public float timer;
}

public class MapManager : MonoBehaviour
{
    [SerializeField] private Cheese[] fruit;
    [SerializeField] private GameObject clearUI;
    private int fruitCnt = 0;
    public bool isClear = false;
    public static MapManager Instance { get; private set; }

    

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void CheckFruit()
    {
        ++fruitCnt;
        if(fruitCnt >= fruit.Length)
        {
            isClear = true;
            clearUI.SetActive(true); 
            Debug.Log("Clear!");
        }
        else
        {
            isClear = false;
            Debug.Log("NotYet");
        }
    }
}
