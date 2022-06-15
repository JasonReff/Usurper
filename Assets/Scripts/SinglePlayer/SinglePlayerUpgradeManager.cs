using UnityEngine;

public class SinglePlayerUpgradeManager : MonoBehaviour
{
    [SerializeField] private SinglePlayerStats _stats;

    private void OnEnable()
    {
        foreach (var upgrade in _stats.Upgrades)
        {
            upgrade.StartUpgrade();
        }
    }

    private void OnDisable()
    {
        foreach (var upgrade in _stats.Upgrades)
        {
            upgrade.EndUpgrade();
        }
    }
}