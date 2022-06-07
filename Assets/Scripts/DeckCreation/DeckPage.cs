using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckPage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private StartingDeck _deck;
    [SerializeField] private List<DeckCard> _cards = new List<DeckCard>();
    [SerializeField] private DeckCard _cardPrefab, _kingCard;
    [SerializeField] private Transform _deckPanel;
    [SerializeField] private GameObject _saveButton;

    public static Action OnDeckSaved;


    private void OnEnable()
    {
        DeckCard.OnDeckCardClicked += RemoveCard;
        CollectionCard.OnCardClicked += AddCard;
        SavedDeckUI.OnDeckClicked += LoadDeck;
    }

    private void OnDisable()
    {
        DeckCard.OnDeckCardClicked -= RemoveCard;
        CollectionCard.OnCardClicked -= AddCard;
        SavedDeckUI.OnDeckClicked -= LoadDeck;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CollectionCard.OnCardDropped += AddCard;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CollectionCard.OnCardDropped -= AddCard;
    }

    private void AddCard(CollectionCard collectionCard)
    {
        if (_deck == null)
            return;
        if (collectionCard.IsKingCard)
        {
            _kingCard.SetCard(collectionCard);
            return;
        }
        if (_cards.Count >= 9)
            return;
        var card = Instantiate(_cardPrefab, _deckPanel);
        card.SetCard(collectionCard);
        _cards.Add(card);
        UpdateButton();
    }

    public void RemoveCard(DeckCard card)
    {
        if (card.IsKingCard)
            return;
        _cards.Remove(card);
        Destroy(card.gameObject);
        UpdateButton();
    }

    public void SaveDeck()
    {
        var units = new List<UnitData>();
        for (int i = 0; i < _cards.Count; i++)
        {
            var data = _cards[i].Data;
            units.Add(data);
        }
        _deck.SetDeck(units);
        _deck.King = _kingCard.Data;
        OnDeckSaved?.Invoke();
    }

    private void ClearPanel()
    {
        if (_cards.Count == 0)
            return;
        for (int i = _cards.Count - 1; i >= 0; i--)
        {
            var card = _cards[i];
            RemoveCard(card);
        }
    }

    private void LoadDeck(SavedDeckUI savedDeck)
    {
        _deck = savedDeck.Deck;
        ClearPanel();
        _kingCard.SetCard(savedDeck.Deck.King);
        foreach (var unit in savedDeck.Deck.Deck)
        {
            var card = Instantiate(_cardPrefab, _deckPanel);
            card.SetCard(unit);
            _cards.Add(card);
        }
        UpdateButton();
    }

    private void UpdateButton()
    {
        if (_cards.Count == 9)
        {
            _saveButton.SetActive(true);
        }
        else _saveButton.SetActive(false);
    }
}
