using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrivateMatchSettingsUI : MonoBehaviour
{
    [SerializeField] private PrivateMatchSettings _settings;

    private void Start()
    {
        _settings.ResetSettings();
    }

    public void SetTime(int time)
    {
        _settings.TimePerPlayer = time;
    }

    public void SetNoPreference()
    {
        _settings.IsFactionPreferred = false;
    }

    public void SetPreferredWhite()
    {
        _settings.IsFactionPreferred = false;
        _settings.PreferredFaction = UnitFaction.Player;
    }

    public void SetPreferredBlack()
    {
        _settings.IsFactionPreferred = true;
        _settings.PreferredFaction = UnitFaction.Enemy;
    }
}