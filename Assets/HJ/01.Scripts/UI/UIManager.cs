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
            SoundManager.Instance.PlaySFX("Click");
            SceneLoadManager.Instance.LoadScene(nextSceneName);
        }

        public void HandleExit()
        {
            Application.Quit();
        }

        public void HandleRestart()
        {
            SoundManager.Instance.PlaySFX("Click");
            SceneLoadManager.Instance.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void HandleMapScene()
        {
            //�������� ������ ������ ����
            SoundManager.Instance.PlaySFX("Click");
            SceneLoadManager.Instance.LoadScene("Stage");
            Time.timeScale = 1;
        }

        public void HandleOpenVolum()
        {
            soundUI.SetActive(true);
        }

        public void HandleCloseVolum()
        {
            soundUI.SetActive(false);
        }

        private void ActiveSetting()
        {
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

        public void HandleSetting()
        {
            SoundManager.Instance.PlaySFX("Click");
            ActiveSetting();
        }
    }
}
