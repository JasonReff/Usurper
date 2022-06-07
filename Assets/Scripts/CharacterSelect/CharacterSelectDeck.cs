using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterSelectDeck : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] protected StartingDeck _deck;
    protected Vector2 _startingPosition;
    protected int _transformHierarchyPosition;
    public static event Action<StartingDeck> OnDeckDropped;
    [SerializeField] protected Image _icon;
    private Camera _main;

    public StartingDeck Deck { get => _deck; set => _deck = value; }

    private void Awake()
    {
        _main = Camera.main;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _transformHierarchyPosition = transform.parent.GetSiblingIndex();
        transform.parent.SetAsLastSibling();
        _startingPosition = transform.position;
        _icon.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var screenPosition = eventData.position;
        var worldPosition = _main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 0));
        transform.position = (Vector2)worldPosition;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        transform.parent.SetSiblingIndex(_transformHierarchyPosition);
        OnDeckDropped?.Invoke(_deck);
        transform.position = _startingPosition;
        _icon.raycastTarget = true;
    }
}
