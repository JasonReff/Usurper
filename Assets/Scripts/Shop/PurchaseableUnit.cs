using Photon.Pun;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseableUnit : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _purchaseButton, _highlight;
    [SerializeField] private Image _card, _icon, _moveset, _cost;
    [SerializeField] private CustomButton _purchase;
    private Sprite _cardSprite;
    [SerializeField] private TextMeshProUGUI _costTextbox, _nameTextbox;
    [SerializeField] private CardColors _cardColors;
    [SerializeField] private Color _whiteTextColor, _blackTextColor;
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
        LoadCardSprite(faction, data.UnitClass);
        _moveset.sprite = data.Moveset;
        Cost = data.Cost;
        _costTextbox.text = Cost.ToString();
        _nameTextbox.text = data.UnitName;
        Unit = data.Unit;
    }

    public void LoadCardSprite(UnitFaction faction, UnitClass unitClass)
    {
        CardColorSet cardColorSet = _cardColors.Neutral;
        switch (unitClass)
        {
            case UnitClass.Default:
                break;
            case UnitClass.Kingdom:
                cardColorSet = _cardColors.Kingdom;
                break;
            case UnitClass.Shogunate:
                cardColorSet = _cardColors.Shogunate;
                break;
            case UnitClass.Empire:
                cardColorSet = _cardColors.Empire;
                break;
        }
        if (faction == UnitFaction.Player)
        {
            _cardSprite = cardColorSet.WhiteCard;
            _nameTextbox.color = _whiteTextColor;
        }
        else
        {
            _cardSprite = cardColorSet.BlackCard;
            _nameTextbox.color = _blackTextColor;
        }
        _cost.sprite = cardColorSet.Cost;
        _purchase.SetColors(cardColorSet);
        _card.sprite = _cardSprite;

    }

    public virtual void SelectUnit()
    {
        OnUnitSelected?.Invoke(this);
    }

    private void HighlightCard(PurchaseableUnit purchaseableUnit)
    {
        if (purchaseableUnit == this)
        {
            _highlight.SetActive(true);
        }
        else _highlight.SetActive(false);
    }

    private void ClearHighlight()
    {
        _card.sprite = _cardSprite;
    }

    public void HidePurchase()
    {
        _purchaseButton.SetActive(false);
    }

    public void ShowPurchase()
    {
        _purchaseButton.SetActive(true);
    }
}
