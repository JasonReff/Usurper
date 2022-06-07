using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CollectionCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] protected Image _card, _unit, _moveset;
    [SerializeField] private TextMeshProUGUI _cost;
    [SerializeField] private GameObject _costImage;
    protected UnitData _data;
    private string _name;
    protected int _transformHeirarchyPosition;
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
        if (IsKingCard)
            _costImage.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _transformHeirarchyPosition = transform.parent.GetSiblingIndex();
        transform.parent.SetSiblingIndex(4);
        _startingPosition = transform.position;
        _card.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var screenPosition = eventData.position;
        var worldPosition = _main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 0));
        transform.position = (Vector2)worldPosition;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        transform.parent.SetSiblingIndex(_transformHeirarchyPosition);
        OnCardDropped?.Invoke(this);
        transform.position = _startingPosition;
        _card.raycastTarget = true;
    }

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
