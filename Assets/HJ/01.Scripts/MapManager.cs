using HJ;
using System;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Fruit[] fruit;
    [SerializeField] private GameObject clearUI;
    private int fruitCnt;

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
            clearUI.SetActive(true);
        }
    }
}
