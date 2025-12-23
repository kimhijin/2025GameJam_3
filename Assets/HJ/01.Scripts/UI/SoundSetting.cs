using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundSetting : MonoBehaviour
{
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider bgmSlider;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        SoundManager.Instance.LoadVolum();
        sfxSlider.value = SoundManager.Instance.sfxVolume;
        bgmSlider.value = SoundManager.Instance.bgmVolume;
    }

    private void OnDisable()
    {
        if(SoundManager.Instance != null)
            SoundManager.Instance.SaveVolum();
    }

    public void ChangeSFX()
    {
        float volum = sfxSlider.value;
        SoundManager.Instance.SetSFXVolume(sfxSlider.value);
    }

    public void ChangeBGM()
    {
        float volum = bgmSlider.value;
        SoundManager.Instance.SetBGMVolume(volum);
    }
}
