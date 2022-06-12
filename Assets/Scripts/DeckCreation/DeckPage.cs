using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckPage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private StartingDeck _deck;
    [SerializeField] private List<DeckCard> _cards = new List<DeckCard>();
    [SerializeField] private DeckCard _cardPrefab, _kingCard;
    [SerializeField] private Transform _deckPanel;
    [SerializeField] private GameObject _saveButton, _cancelPanel;
    private bool _isCurrentDeckSaved = true;

    public static Action OnDeckSaved;

    private void OnEnable()
    {
        DeckCard.OnDeckCardClicked += RemoveCard;
        CollectionCard.OnCardClicked += AddCard;
        SavedDeckUI.OnDeckClicked += LoadDeck;
        SavedDeckUI.OnDeckCleared += OnDeckCleared;
    }

    private void OnDisable()
    {
        DeckCard.OnDeckCardClicked -= RemoveCard;
        CollectionCard.OnCardClicked -= AddCard;
        SavedDeckUI.OnDeckClicked -= LoadDeck;
        SavedDeckUI.OnDeckCleared -= OnDeckCleared;
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
        _isCurrentDeckSaved = false;
    }

    public void RemoveCard(DeckCard card)
    {
        if (card.IsKingCard)
            return;
        _cards.Remove(card);
        Destroy(card.gameObject);
        UpdateButton();
        _isCurrentDeckSaved = false;
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
        _isCurrentDeckSaved = true;
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

    public void LoadDeck(SavedDeckUI savedDeck)
    {
        if (_isCurrentDeckSaved == false && _deck != null)
        {
            ShowCancelPanel();
            return;
        }
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
        _isCurrentDeckSaved = true;
    }

    private void ShowCancelPanel()
    {
        _cancelPanel.SetActive(true);
    }

    public void Cancel()
    {
        _isCurrentDeckSaved = true;
        _cancelPanel.SetActive(false);
    }

    private void UpdateButton()
    {
        if (_cards.Count == 9)
        {
            _saveButton.SetActive(true);
        }
        else _saveButton.SetActive(false);
    }

    private void OnDeckCleared(SavedDeckUI deckUI)
    {
        if (deckUI.Deck == _deck)
        {
            ClearPanel();
            _deck = null;
        }
    }
}
