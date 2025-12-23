using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float Timer { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("dafasdfdasfsdaf");
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        Timer = 0;
    }

    private void Update()
    {
        Timer += Time.deltaTime;
    }
}