using System;
using UnityEngine.EventSystems;

public class DeckCard : CollectionCard
{
    public static Action<DeckCard> OnDeckCardDropped;
    public static Action<DeckCard> OnDeckCardClicked;

    public void SetCard(CollectionCard card)
    {
        SetCard(card.Data);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        transform.parent.SetSiblingIndex(_transformHeirarchyPosition);
        OnDeckCardDropped?.Invoke(this);
        transform.position = _startingPosition;
        _card.raycastTarget = true;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        OnDeckCardClicked?.Invoke(this);
    }
}