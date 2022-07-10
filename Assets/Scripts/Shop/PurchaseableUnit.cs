using Photon.Pun;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PurchaseableUnit : MonoBehaviourPunCallbacks, IPointerClickHandler
{
    [SerializeField] private GameObject _purchaseButton, _highlight;
    [SerializeField] private Image _card, _icon, _moveset, _cost, _usesImage;
    [SerializeField] private CustomButton _purchase;
    private Sprite _cardSprite;
    [SerializeField] private TextMeshProUGUI _costTextbox, _nameTextbox, _usesTextbox;
    [SerializeField] private CardColors _cardColors;
    [SerializeField] private Color _whiteTextColor, _blackTextColor;
    public UnitData Data;
    public UnitCard Card;
    public Unit Unit;
    public int Cost, Uses;
    public bool Purchaseable;

    public Sprite GetUnitSprite { get => _icon.sprite; }
    

    public static Action<PurchaseableUnit> OnUnitSelected;

    private void OnEnable()
    {
        OnUnitSelected += HighlightCard;
        BoardVisualizer.OnVirtualBoardCreated += ClearHighlight;
    }

    private void OnDisable()
    {
        OnUnitSelected -= HighlightCard;
        BoardVisualizer.OnVirtualBoardCreated -= ClearHighlight;
    }

    public void LoadUnitData(UnitCard card, UnitFaction faction)
    {
        Data = card.UnitData;
        Card = card;
        if (faction == UnitFaction.White)
            _icon.sprite = card.UnitData.PlayerSprite;
        else
            _icon.sprite = card.UnitData.EnemySprite;
        LoadCardSprite(faction, card.UnitData.UnitClass);
        _moveset.sprite = card.UnitData.Moveset;
        Cost = card.UnitData.Cost;
        Uses = card.NumberOfUses;
        _costTextbox.text = Cost.ToString();
        _nameTextbox.text = card.UnitData.UnitName;
        _usesTextbox.text = Uses.ToString();
        Unit = card.UnitData.Unit;
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
        if (faction == UnitFaction.White)
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
        _usesImage.sprite = cardColorSet.Uses;
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Purchaseable)
            SelectUnit();
    }
}
