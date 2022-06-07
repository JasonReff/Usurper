using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CollectionPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private DeckPage _deckPage;
    public void OnPointerEnter(PointerEventData eventData)
    {
        DeckCard.OnDeckCardDropped += RemoveCard;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DeckCard.OnDeckCardDropped -= RemoveCard;
    }

    private void RemoveCard(DeckCard card)
    {
        _deckPage.RemoveCard(card);
    }
}
