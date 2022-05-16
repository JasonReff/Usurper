using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseableUnit : MonoBehaviour
{
    [SerializeField] private Image _icon, _moveset;
    [SerializeField] private TextMeshProUGUI _costTextbox;
    public UnitData Data;
    public Unit Unit;
    public int Cost;

    public Sprite GetUnitSprite { get => _icon.sprite; }
    

    public static Action<PurchaseableUnit> OnUnitSelected;

    public void LoadUnitData(UnitData data, UnitFaction faction)
    {
        Data = data;
        if (faction == UnitFaction.Player)
            _icon.sprite = data.PlayerSprite;
        else
            _icon.sprite = data.EnemySprite;
        _moveset.sprite = data.Moveset;
        Cost = data.Cost;
        _costTextbox.text = Cost.ToString();
        Unit = data.Unit;
    }

    public void SelectUnit()
    {
        OnUnitSelected?.Invoke(this);
    }
}
