using UnityEngine;
using UnityEngine.SceneManagement;

namespace HJ
{


    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject soundUI; //
        [SerializeField] private GameObject settingUI;
        [SerializeField] private GameObject clearUI;
        [SerializeField] private string nextSceneName;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !clearUI.activeSelf)
                ActiveSetting();    
        }


        public void HandleNextScene()
        {
            SceneManager.LoadScene(nextSceneName);
        }

        public void HandleExit()
        {
            Application.Quit();
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

        public void HandleOpenVolum()
        {
            soundUI.SetActive(true);
        }

        public void HandleCloseVolum()
        {
            soundUI.SetActive(false);
        }

        public void ActiveSetting()
        {
            Debug.Log("Input Esc");
            if(settingUI.activeSelf == false)
            {
                settingUI.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                settingUI.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }
}
