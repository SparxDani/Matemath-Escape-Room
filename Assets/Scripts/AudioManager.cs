using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static bool isMusicMuted = false;
    public static bool isSFXMuted = false;

    public AudioMixer audioMixer;

    public List<Image> music_sfx_Images;
    public List<Sprite> musicSprites,sfxSprites;

    public void Start()
    {
        music_sfx_Images[0].sprite = isMusicMuted ? musicSprites[1]: musicSprites[0];
        music_sfx_Images[1].sprite = isSFXMuted ? sfxSprites[1]: sfxSprites[0];
    }

    // Métodos para silenciar y restaurar música
    public void ToggleMusic()
    {
        isMusicMuted = !isMusicMuted;
        if (isMusicMuted)
        {
            music_sfx_Images[0].sprite = musicSprites[1];
            audioMixer.SetFloat("musicVolume", -80f);  // Silenciar música
        }
        else
        {
            music_sfx_Images[0].sprite = musicSprites[0];
            audioMixer.SetFloat("musicVolume", 0f);    // Restaurar volumen por defecto
        }
    }

    // Métodos para silenciar y restaurar efectos de sonido
    public void ToggleSFX()
    {
        isSFXMuted = !isSFXMuted;
        if (isSFXMuted)
        {
            music_sfx_Images[1].sprite = sfxSprites[1];
            audioMixer.SetFloat("sfxVolume", -80f);    // Silenciar efectos de sonido
        }
        else
        {
            music_sfx_Images[1].sprite = sfxSprites[0];
            audioMixer.SetFloat("sfxVolume", 0f);      // Restaurar volumen por defecto
        }
    }

    // Método para aplicar el estado guardado al reiniciar una escena
    public void ApplyAudioSettings()
    {
        music_sfx_Images[0].sprite = isMusicMuted ? musicSprites[1]: musicSprites[0];
        music_sfx_Images[1].sprite = isSFXMuted ? sfxSprites[1]: sfxSprites[0];
        audioMixer.SetFloat("musicVolume", isMusicMuted ? -80f : 0f);
        audioMixer.SetFloat("sfxVolume", isSFXMuted ? -80f : 0f);
    }
}
