using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDeckManager : MonoBehaviour
{
    [SerializeField] private CustomDeckCollection _collection;
    [SerializeField] private Transform _deckList;
    [SerializeField] private SavedDeckUI _deckPrefab;
    private List<SavedDeckUI> _decks = new List<SavedDeckUI>();

    private void Start()
    {
        LoadDecks();
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
        var deckUI = Instantiate(_deckPrefab, _deckList);
        deckUI.Deck = _collection.Decks[_decks.Count];
        _decks.Add(deckUI);
    }
}
