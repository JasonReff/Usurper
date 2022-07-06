using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SinglePlayerBossManager : MonoBehaviour
{
    [SerializeField] private Board _board;
    [SerializeField] private GameObject _enemyAIManager, _bossDisplayUI;
    [SerializeField] private EnemyShopManager _shop;
    [SerializeField] private SinglePlayerStats _stats;
    [SerializeField] private List<SpecialEnemyManager> _minibossManagers = new List<SpecialEnemyManager>();
    [SerializeField] private List<SpecialEnemyManager> _bossManagers = new List<SpecialEnemyManager>();
    [SerializeField] private TextMeshProUGUI _bossNameTextbox;

    private void Awake()
    {
        if (_stats.Round % 3 == 0)
        {
            EnsureBoardExists();
            ReplaceAIWithMiniboss();
        }
        else if (_stats.Round == 10)
        {
            EnsureBoardExists();
            ReplaceAIWithBoss();
        }
    }

    private void EnsureBoardExists()
    {
        if (Board.Instance == null)
        {
            Board.Instance = _board;
        }
    }

    public void ReplaceAIWithMiniboss()
    {
        Destroy(_enemyAIManager);
        List<SpecialEnemyManager> newEncounters = _minibossManagers.Where(t => !_stats.MinibossList.Contains(t)).ToList();
        var miniboss = newEncounters.Rand();
        var AIManager = Instantiate(miniboss);
        AIManager.Shop = _shop;
        AIManager.ResetDeck();
        _stats.MinibossList.Add(miniboss);
        DisplayBossName(miniboss, false);
    }

    public void ReplaceAIWithBoss()
    {
        Destroy(_enemyAIManager);
        var boss = _bossManagers.Rand();
        var AIManager = Instantiate(boss);
        AIManager.Shop = _shop;
        AIManager.ResetDeck();
        DisplayBossName(boss, true);
    }

    private void DisplayBossName(SpecialEnemyManager boss, bool isBoss)
    {
        StartCoroutine(DisplayCoroutine());

        IEnumerator DisplayCoroutine()
        {
            _bossDisplayUI.SetActive(true);
            if (!isBoss)
                _bossNameTextbox.text = "Miniboss: " + boss.BossName;
            else
                _bossNameTextbox.text = "Boss: " + boss.BossName;
            yield return new WaitForSeconds(1f);
            _bossDisplayUI.SetActive(false);
        }
    }
}
