using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CollectionCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] protected Image _card, _unit, _moveset, _costImage, _usesImage;
    [SerializeField] private TextMeshProUGUI _cost, _nameTextbox, _usesTextbox;
    [SerializeField] private CardColors _cardColors;
    protected UnitData _data;
    private string _name;
    protected int _parentHeirarchyPosition, _cardHeirarchyPosition;
    protected Vector2 _startingPosition;
    private Camera _main;
    public static Action<CollectionCard> OnCardDropped;
    public static Action<CollectionCard> OnCardClicked;
    public static Action<string> OnMouseOver;
    public static Action<string> OnMouseExit;
    public bool IsKingCard;

    public UnitData Data { get => _data; }
    private void Awake()
    {
        _main = Camera.main;
    }

    public void SetCard(UnitData data)
    {
        _data = data;
        _unit.sprite = data.PlayerSprite;
        _moveset.sprite = data.Moveset;
        _name = data.UnitName;
        _cost.text = data.Cost.ToString();
        IsKingCard = data.IsKing;
        _nameTextbox.text = _name;
        _usesTextbox.text = data.StartingUses.ToString();
        LoadCardSprite(data.UnitClass);
        if (IsKingCard)
        {
            _costImage.enabled = false;
            _cost.enabled = false;
            _usesImage.enabled = false;
            _usesTextbox.enabled = false;
        }
            
    }

    public void LoadCardSprite(UnitClass unitClass)
    {
        Sprite cardSprite = null;
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
        cardSprite = cardColorSet.WhiteCard;
        _costImage.sprite = cardColorSet.Cost;
        _usesImage.sprite = cardColorSet.Uses;
        _card.sprite = cardSprite;
    }

    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //    _cardHeirarchyPosition = transform.GetSiblingIndex();
    //    _parentHeirarchyPosition = transform.parent.GetSiblingIndex();
    //    transform.parent.SetSiblingIndex(4);
    //    transform.SetAsLastSibling();
    //    _startingPosition = transform.position;
    //    _card.raycastTarget = false;
    //}

    //public void OnDrag(PointerEventData eventData)
    //{
    //    var screenPosition = eventData.position;
    //    transform.position = (Vector2)screenPosition;
    //}

    //public virtual void OnEndDrag(PointerEventData eventData)
    //{
    //    transform.SetSiblingIndex(_cardHeirarchyPosition);
    //    transform.parent.SetSiblingIndex(_parentHeirarchyPosition);
    //    OnCardDropped?.Invoke(this);
    //    transform.position = _startingPosition;
    //    _card.raycastTarget = true;
    //}

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseOver?.Invoke(_name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExit?.Invoke(_name);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        OnCardClicked?.Invoke(this);
    }
}
