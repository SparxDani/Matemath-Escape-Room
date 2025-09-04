using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (FindObjectsOfType<AudioManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Start()
    {
        InitializeAudioManager();
    }

    public void InitializeAudioManager()
    {
        LoadAudioSettings();
        musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXSliderChanged);
    }

    private void OnMusicSliderChanged(float volume)
    {
        SetMusicVolume(volume);
        PlayerPrefs.SetFloat("musicVolume", volume);
        PlayerPrefs.Save();
    }

    private void OnSFXSliderChanged(float volume)
    {
        SetSFXVolume(volume);
        PlayerPrefs.SetFloat("sfxVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float volume)
    {
        if (volume <= 0.01f)
        {
            audioMixer.SetFloat("musicVolume", -80f);
        }
        else
        {
            audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
        }
    }

    public void SetSFXVolume(float volume)
    {
        if (volume <= 0.01f)
        {
            audioMixer.SetFloat("sfxVolume", -80f);
        }
        else
        {
            audioMixer.SetFloat("sfxVolume", Mathf.Log10(volume) * 20);
        }
    }

    public void LoadAudioSettings()
    {
        float musicVolume = PlayerPrefs.GetFloat("musicVolume", 1.0f);
        float sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1.0f);
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;
    }
}