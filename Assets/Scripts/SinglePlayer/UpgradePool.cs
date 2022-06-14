using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Pool")]
public class UpgradePool : ScriptableObject
{
    public List<SinglePlayerUpgrade> Upgrades;
}