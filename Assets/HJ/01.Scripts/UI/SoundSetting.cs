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
        SoundManager.Instance.SetSFXVolume(volum);
    }

    public void ChangeBGM()
    {
        float volum = bgmSlider.value;
        SoundManager.Instance.SetSFXVolume(volum);
    }
}
