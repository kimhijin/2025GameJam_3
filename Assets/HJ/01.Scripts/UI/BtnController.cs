using UnityEngine;
using UnityEngine.SceneManagement;

namespace HJ
{
    public class BtnController : MonoBehaviour
    {


        public void HandleStartBtn()
        {
            Debug.Log("너는 눌렀다 Start버튼을");
            SceneManager.LoadScene("맵 씬으로 이동");
        }

        public void HandleSettingBtn()
        {
            Debug.Log("너는 눌렀다 Setting버튼을");
            //세팅 창 나타나기
        }

        public void HandleExitBtn()
        {
            Debug.Log("너는 눌렀다 Exit버튼을");
            Application.Quit();
        }
    }
}
