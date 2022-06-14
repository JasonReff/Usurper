using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpecialEncounterManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyAIManager;
    [SerializeField] private EnemyShopManager _shop;
    [SerializeField] private SinglePlayerStats _stats;
    [SerializeField] private List<SpecialEnemyManager> _specialAIManagers = new List<SpecialEnemyManager>();

    private void Awake()
    {
        if (_stats.Round % 3 == 0)
        {
            ReplaceAI();
        }
    }

    public void ReplaceAI()
    {
        Destroy(_enemyAIManager);
        List<SpecialEnemyManager> newEncounters = _specialAIManagers.Where(t => !_stats.BossList.Contains(t)).ToList();
        var boss = newEncounters.Rand();
        var AIManager = Instantiate(boss);
        AIManager.Shop = _shop;
        AIManager.ResetDeck();
        _stats.BossList.Add(boss);
    }
}
