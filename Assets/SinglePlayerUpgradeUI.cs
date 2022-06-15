using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SinglePlayerUpgradeUI : MonoBehaviour
{
    [SerializeField] private SinglePlayerUpgrade _upgrade;
    [SerializeField] private Image _upgradeImage;
    [SerializeField] private TextMeshProUGUI _nameTextbox;
    public static Action<SinglePlayerUpgrade> OnUpgradeChosen;

    public void SetUpgrade(SinglePlayerUpgrade upgrade)
    {
        _upgrade = upgrade;
        _upgradeImage.sprite = _upgrade.UpgradeSprite;
        _nameTextbox.text = _upgrade.UpgradeName;
    }

    public void UpgradeSelected()
    {
        OnUpgradeChosen?.Invoke(_upgrade);
    }
}
