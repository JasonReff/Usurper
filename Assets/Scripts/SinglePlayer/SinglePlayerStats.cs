using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SinglePlayerStats")]
public class SinglePlayerStats : ScriptableObject
{
    public int Round = 1;
    public List<SpecialEnemyManager> MinibossList = new List<SpecialEnemyManager>();
    public List<SinglePlayerUpgrade> Upgrades = new List<SinglePlayerUpgrade>();

    public void ResetStats()
    {
        Round = 1;
        MinibossList.Clear();
        Upgrades.Clear();
    }

    public void PracticeMode()
    {
        Round = 0;
        MinibossList.Clear();
        Upgrades.Clear();
    }

    public void AddUpgrade(SinglePlayerUpgrade upgrade)
    {
        Upgrades.Add(upgrade);
    }
}
