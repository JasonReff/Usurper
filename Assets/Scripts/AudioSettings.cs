using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/AudioSettings")]
public class AudioSettings : ScriptableObject
{
    public float _musicVolume = 0.5f;
    public float _effectsVolume = 0.5f;

    public void SetMusicVolume(float volume)
    {
        _musicVolume = volume;
        OnAudioSettingsChanged?.Invoke();
    }

    public void SetEffectsVolume(float volume)
    {
        _effectsVolume = volume;
        OnAudioSettingsChanged?.Invoke();
    }

    public static Action OnAudioSettingsChanged;
}