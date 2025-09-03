using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Button saveButton; // Botón para guardar configuraciones
    public Button resetButton; // Botón para restablecer valores predeterminados

    public void Awake()
    {
        // Evitar que el objeto se destruya al cargar nuevas escenas
        DontDestroyOnLoad(gameObject);

        // Verificar si ya existe una instancia de este objeto
        if (FindObjectsOfType<AudioManager>().Length > 1)
        {
            Destroy(gameObject); // Destruir duplicados si ya existe uno
            return;
        }
    }

    public void Start()
    {
        InitializeAudioManager();
    }

    public void InitializeAudioManager()
    {
        // Cargar configuraciones de audio
        LoadAudioSettings();

        // Agregar listeners a los sliders
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        // Agregar listeners a los botones
        saveButton.onClick.AddListener(SaveAudioSettings);
        resetButton.onClick.AddListener(ResetToDefault);
    }

    public void SetMusicVolume(float volume)
    {
        // Manejar el caso en el que el volumen sea 0
        if (volume <= 0.01f)
        {
            audioMixer.SetFloat("MusicVolumen", -80f); // Silencio total
        }
        else
        {
            audioMixer.SetFloat("MusicVolumen", Mathf.Log10(volume) * 20); // Escala logarítmica
        }

        //PlayerPrefs.SetFloat("MusicVolumen", volume);
        //PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        // Manejar el caso en el que el volumen sea 0
        if (volume <= 0.01f)
        {
            audioMixer.SetFloat("SFXVolumen", -80f); // Silencio total
        }
        else
        {
            audioMixer.SetFloat("SFXVolumen", Mathf.Log10(volume) * 20); // Escala logarítmica
        }

        //PlayerPrefs.SetFloat("SFXVolumen", volume);
        //PlayerPrefs.Save();
    }

    public void LoadAudioSettings()
    {
        // Cargar los valores guardados de PlayerPrefs o usar valores predeterminados
        float musicVolume = PlayerPrefs.GetFloat("MusicVolumen", 1.0f); // Valor predeterminado: 1.0 (máximo)
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolumen", 1.0f); // Valor predeterminado: 1.0 (máximo)

        // Establecer los valores iniciales del AudioMixer
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);

        // Inicializar los sliders con los valores cargados
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;
    }

    public void SaveAudioSettings()
    {
        // Guardar los valores actuales de los sliders en PlayerPrefs
        PlayerPrefs.SetFloat("MusicVolumen", musicSlider.value);
        PlayerPrefs.SetFloat("SFXVolumen", sfxSlider.value);
        PlayerPrefs.Save();

        Debug.Log("Configuraciones de audio guardadas.");
    }

    public void ResetToDefault()
    {
        // Restablecer los valores predeterminados
        float defaultVolume = 1.0f; // Máximo volumen

        // Establecer valores predeterminados en el AudioMixer
        SetMusicVolume(defaultVolume);
        SetSFXVolume(defaultVolume);

        // Actualizar los sliders
        musicSlider.value = defaultVolume;
        sfxSlider.value = defaultVolume;

        Debug.Log("Valores de audio restablecidos a los predeterminados.");
    }
}