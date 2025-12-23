using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

[System.Serializable]
class SoundClip
{
    [field:SerializeField]public string Name { get; private set; }
    [field:SerializeField]public AudioClip Clip { get; private set; }
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioClip bgmClip;
    public float bgmVolume { get; private set;}

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip[] clipes;
    public float sfxVolume { get; private set; }

    private Dictionary<string, AudioClip> clipDic = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        foreach (var item in clipes)
        {
            Debug.Log(item.name);
            clipDic[item.name] = item;
        }

        LoadVolum();
        PlayBgm(bgmClip);
    }

    public void PlaySFX(string clipName)
    {
        Debug.Assert(clipDic.ContainsKey(clipName), clipName + " <--- 이거 잘못 썼음요");
        sfxSource.PlayOneShot(clipDic[clipName]);
    }

    public void PlayBgm(AudioClip clip)
    {

        bgmSource.Stop();          // 기존 BGM 정지
        bgmSource.clip = clip;     // 클립 지정
        bgmSource.loop = true;     // BGM은 루프
        bgmSource.Play();
    }

    public void StopBgm(string scecneName)
    {
        DOTween.To(x => bgmSource.volume = x, bgmSource.volume, 0f, 2f).OnComplete(() =>
        {
            SceneManager.LoadScene(scecneName);
        }).SetUpdate(true);
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
        PlayerPrefs.SetFloat("Bgm", bgmVolume);
        PlayerPrefs.SetFloat("Sfx", sfxVolume);
    }

    public void LoadVolum()
    {
        SetSFXVolume(PlayerPrefs.GetFloat("Sfx", 0.5f));
        SetBGMVolume(PlayerPrefs.GetFloat("Bgm", 0.5f));
    }
}
