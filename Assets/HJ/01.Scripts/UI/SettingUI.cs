using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField] private Slider sfxSlider;
    private float sfxVolum;
    [SerializeField] private Slider bgmSlider;
    private float bgmVolum;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void ActiveOption()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        if (gameObject.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void ChangeSFX()
    {
        float volum = sfxSlider.value;
        SoundManager.Instance.SetSFXVolume(volum);
    }

    public void ChangeBGM()
    {
        float volum = bgmSlider.value;
        SoundManager.Instance.SetSFXVolume(volum);
    }
}
