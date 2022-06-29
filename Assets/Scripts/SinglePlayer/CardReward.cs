using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardReward : MonoBehaviour
{
    [SerializeField] private UnitData _unitData;
    [SerializeField] private Image _unitImage, _moveset, _card, _cost;
    [SerializeField] private TextMeshProUGUI _costText, _nameTextbox;
    [SerializeField] private CustomButton _chooseButton;
    [SerializeField] private CardColors _cardColors;
    [SerializeField] private Color _whiteTextColor, _blackTextColor;
    [SerializeField] private GameObject _highlight;
    private Sprite _cardSprite;
    public static Action<CardReward> OnCardSelected;

    public UnitData UnitData { get => _unitData; }

    private void OnEnable()
    {
        RemovableCard.OnCardSelected += RemoveHighlight;
        OnCardSelected += SetHighlight;
    }

    private void OnDisable()
    {
        RemovableCard.OnCardSelected -= RemoveHighlight;
        OnCardSelected -= SetHighlight;
    }

    public void SelectUnit()
    {
        OnCardSelected?.Invoke(this);
    }

    public void SetUnit(UnitData data)
    {
        _unitData = data;
        _unitImage.sprite = data.PlayerSprite;
        LoadCardSprite(UnitFaction.Player, data.UnitClass);
        _moveset.sprite = data.Moveset;
        _costText.text = data.Cost.ToString();
        _nameTextbox.text = data.UnitName;
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
        _chooseButton.SetColors(cardColorSet);
        _card.sprite = _cardSprite;
    }

    private void SetHighlight(CardReward cardReward)
    {
        if (cardReward != this)
            _highlight.SetActive(false);
        else _highlight.SetActive(true);
    }

    private void RemoveHighlight(RemovableCard card)
    {
        _highlight.SetActive(false);
    }
}