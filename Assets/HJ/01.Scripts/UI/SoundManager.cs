using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class SoundClip
{
    [field:SerializeField]public string Name { get; private set; }
    [field:SerializeField]public AudioClip Clip { get; private set; }
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField]private AudioSource bgmSource;
    [SerializeField] private AudioClip bgmClip;
    public float bgmVolume { get; private set;}

    [SerializeField]private AudioSource sfxSource;
    [SerializeField] private AudioClip[] clipes;
    public float sfxVolume { get; private set; }

    private Dictionary<string, AudioClip> clipDic = new();

    private void Awake()
    {
        if (Instance != null)
            Instance = this;
        else
            Destroy(gameObject);

        foreach (var item in clipes)
        {
            clipDic[item.name] = item;
        }

        PlayBgm(bgmClip);
    }

    public void PlaySFX(string clipName)
    {
        sfxSource.PlayOneShot(clipDic[clipName]);
    }

    public void PlayBgm(AudioClip clip)
    {
        bgmSource.PlayOneShot(clip);
    }

    public void StopBgm()
    {
        bgmSource.Stop();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        sfxSource.volume = volume;
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        bgmSource.volume = volume;
    }

    public void SaveVolum()
    {
        PlayerPrefs.SetFloat("Bgm", bgmSource.volume);
        PlayerPrefs.SetFloat("Sfx", sfxSource.volume);
    }
}
