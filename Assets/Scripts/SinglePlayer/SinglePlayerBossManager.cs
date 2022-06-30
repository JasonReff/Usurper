using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SinglePlayerBossManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyAIManager, _bossDisplayUI;
    [SerializeField] private EnemyShopManager _shop;
    [SerializeField] private SinglePlayerStats _stats;
    [SerializeField] private List<SpecialEnemyManager> _specialAIManagers = new List<SpecialEnemyManager>();
    [SerializeField] private TextMeshProUGUI _bossNameTextbox;

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
        DisplayBossName(boss);
    }

    private void DisplayBossName(SpecialEnemyManager boss)
    {
        StartCoroutine(DisplayCoroutine());

        IEnumerator DisplayCoroutine()
        {
            _bossDisplayUI.SetActive(true);
            _bossNameTextbox.text = "Miniboss: " + boss.BossName;
            yield return new WaitForSeconds(1f);
            _bossDisplayUI.SetActive(false);
        }
    }
}
