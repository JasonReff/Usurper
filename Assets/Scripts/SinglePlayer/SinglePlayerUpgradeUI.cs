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
    [SerializeField] private GameObject _highlight;
    public static Action<SinglePlayerUpgrade> OnUpgradeChosen;

    private void OnEnable()
    {
        OnUpgradeChosen += SetHighlight;
    }

    private void OnDisable()
    {
        OnUpgradeChosen -= SetHighlight;
    }

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

    private void SetHighlight(SinglePlayerUpgrade upgrade)
    {
        if (upgrade != this)
            _highlight.SetActive(false);
        else _highlight.SetActive(true);
    }
}
