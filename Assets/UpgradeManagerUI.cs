using System.Collections.Generic;
using UnityEngine;

public class UpgradeManagerUI : MonoBehaviour
{
    [SerializeField] private SinglePlayerUpgradeUI _upgradeUIPrefab;
    [SerializeField] private UpgradePool _pool;
    [SerializeField] private SinglePlayerStats _stats;
    [SerializeField] private GameObject _confirmButton, _roundWonPanel;
    [SerializeField] private List<SinglePlayerUpgradeUI> _upgradeUIs = new List<SinglePlayerUpgradeUI>();
    private SinglePlayerUpgrade _chosenUpgrade;

    private void OnEnable()
    {
        SinglePlayerUpgradeUI.OnUpgradeChosen += OnUpgradeSelected;
    }

    private void OnDisable()
    {
        SinglePlayerUpgradeUI.OnUpgradeChosen -= OnUpgradeSelected;
    }

    private void Awake()
    {
        SetUpgrades();
    }

    public void SetUpgrades()
    {
        var upgradeList = _pool.Upgrades.Pull(3);
        for (int i = 0; i < 3; i++)
        {
            var upgradeUI = _upgradeUIs[i];
            upgradeUI.SetUpgrade(upgradeList[i]);
        }
    }

    private void OnUpgradeSelected(SinglePlayerUpgrade upgrade)
    {
        _chosenUpgrade = upgrade;
        _confirmButton.SetActive(true);
    }

    public void ConfirmUpgrade()
    {
        _stats.Upgrades.Add(_chosenUpgrade);
        _roundWonPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
