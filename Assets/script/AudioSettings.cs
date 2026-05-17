using UnityEngine;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour
{
    public AudioMixer mixer;

    void Start()
    {
        float savedMaster =
            PlayerPrefs.GetFloat("GameVolume", 1f);

        float savedMusic =
            PlayerPrefs.GetFloat("MusicVolume", 1f);

        float savedSFX =
            PlayerPrefs.GetFloat("SFXVolume", 1f);

        SetVolume(savedMaster);
        SetMusicVolume(savedMusic);
        SetSFXVolume(savedSFX);
    }

    // MASTER VOLUME
    public void SetVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1f);

        mixer.SetFloat(
            "Volume",
            Mathf.Log10(volume) * 20f
        );

        PlayerPrefs.SetFloat(
            "GameVolume",
            volume
        );
    }

    // MUSIC VOLUME
    public void SetMusicVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1f);

        mixer.SetFloat(
            "MusicVolume",
            Mathf.Log10(volume) * 20f
        );

        PlayerPrefs.SetFloat(
            "MusicVolume",
            volume
        );
    }

    // SFX VOLUME
    public void SetSFXVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1f);

        mixer.SetFloat(
            "SFXVolume",
            Mathf.Log10(volume) * 20f
        );

        PlayerPrefs.SetFloat(
            "SFXVolume",
            volume
        );
    }
}