using Photon.Pun;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseableUnit : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _purchaseButton;
    [SerializeField] private Image _card, _icon, _moveset;
    [SerializeField] private Sprite _notSelected, _selected;
    [SerializeField] private TextMeshProUGUI _costTextbox;
    public UnitData Data;
    public Unit Unit;
    public int Cost;

    public Sprite GetUnitSprite { get => _icon.sprite; }
    

    public static Action<PurchaseableUnit> OnUnitSelected;

    private void OnEnable()
    {
        OnUnitSelected += HighlightCard;
        BoardVisualizer.OnBoardCreated += ClearHighlight;
    }

    private void OnDisable()
    {
        OnUnitSelected -= HighlightCard;
        BoardVisualizer.OnBoardCreated -= ClearHighlight;
    }

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

    public virtual void SelectUnit()
    {
        OnUnitSelected?.Invoke(this);
    }

    private void HighlightCard(PurchaseableUnit purchaseableUnit)
    {
        if (purchaseableUnit == this)
        {
            _card.sprite = _selected;
        }
        else _card.sprite = _notSelected;
    }

    private void ClearHighlight()
    {
        _card.sprite = _notSelected;
    }

    public void HidePurchase()
    {
        _purchaseButton.SetActive(false);
    }
}
