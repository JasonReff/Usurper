using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private AudioSettings _audioSettings;
    [SerializeField] private Slider _musicSlider, _effectSlider;
    [SerializeField] private bool _kingCanMoveIntoCheck;
    void Start()
    {
        _musicSlider.value = _audioSettings._musicVolume;
        _effectSlider.value = _audioSettings._effectsVolume;
    }

}
