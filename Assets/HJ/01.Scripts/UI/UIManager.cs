using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HJ
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject settingUI;
        [SerializeField] private GameObject clearUI;

        [SerializeField] private string nextSceneName;

        private void Awake()
        {
            MapManager.Instance.OnClear += HandleClearUI;
        }


        private void OnDestroy()
        {
            MapManager.Instance.OnClear -= HandleClearUI;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
                settingUI.SetActive(!settingUI.activeSelf);
        }


        public void HandleNextScene()
        {
            SceneManager.LoadScene(nextSceneName);
        }

        public void HandleRestart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void HandleMapScene()
        {
            //스테이지 고르는 씬으로 ㄱㄱ
            SceneManager.LoadScene("Stage");
        }

        private void HandleClearUI()
        {
            Debug.Log("OpenUI");
            clearUI.SetActive(true);
        }
    }
}
