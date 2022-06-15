using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SinglePlayerUpgradeUI : MonoBehaviour
{
    [SerializeField] private SinglePlayerUpgrade _upgrade;
    [SerializeField] private Image _upgradeImage;
    public static Action<SinglePlayerUpgrade> OnUpgradeChosen;

    public void SetUpgrade(SinglePlayerUpgrade upgrade)
    {
        _upgrade = upgrade;
        _upgradeImage.sprite = _upgrade.UpgradeSprite;
    }

    public void UpgradeSelected()
    {
        OnUpgradeChosen?.Invoke(_upgrade);
    }
}
