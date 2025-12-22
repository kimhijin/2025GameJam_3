using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HJ
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject soundUI; //
        [SerializeField] private GameObject settingUI;
        [SerializeField] private string nextSceneName;

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

        public void HandleOpenSetting()
        {
            soundUI.SetActive(true);
        }

        public void HandleCloseSetting()
        {
            soundUI.SetActive(false);
        }
    }
}
