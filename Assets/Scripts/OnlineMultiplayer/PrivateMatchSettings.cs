using UnityEngine;

[CreateAssetMenu(menuName = "Settings/PrivateMatchSettings")]
public class PrivateMatchSettings : ScriptableObject
{
    public bool IsPrivateMatch = false;
    public int TimePerPlayer = 600;
    public UnitFaction PreferredFaction = UnitFaction.White;
    public bool IsFactionPreferred = false;

    public void ResetSettings()
    {
        IsPrivateMatch = false;
        TimePerPlayer = 600;
        PreferredFaction = UnitFaction.White;
        IsFactionPreferred = false;
    }
}
