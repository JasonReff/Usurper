using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomDeckManager : MonoBehaviour
{
    [SerializeField] private DeckPage _deckPage;
    [SerializeField] private CustomDeckCollection _collection;
    [SerializeField] private Transform _deckList;
    [SerializeField] private SavedDeckUI _deckPrefab;
    private List<SavedDeckUI> _decks = new List<SavedDeckUI>();

    private void Start()
    {
        LoadDecks();
        if (_decks.Count > 0)
            _deckPage.LoadDeck(_decks.First());
    }

    private void LoadDecks()
    {
        _collection.LoadDecks();
        foreach (var deck in _collection.Decks)
        {
            if (deck.Deck.Count > 0)
            {
                var deckUI = Instantiate(_deckPrefab, _deckList);
                deckUI.Deck = deck;
                deckUI.LoadName();
                _decks.Add(deckUI);
            }
        }
    }

    public void CreateNewDeck()
    {
        if (_decks.Count >= 9)
            return;
        var deckUI = Instantiate(_deckPrefab, _deckList);
        deckUI.Deck = _collection.Decks.Where(t => t.Deck.Count == 0).First();
        _decks.Add(deckUI);
    }
}
