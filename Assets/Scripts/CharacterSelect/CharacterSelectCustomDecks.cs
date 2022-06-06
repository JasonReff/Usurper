using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectCustomDecks : MonoBehaviour
{
    [SerializeField] private Transform _deckList;
    [SerializeField] private CharacterSelectCustomDeck _deckPrefab;
    private List<CharacterSelectCustomDeck> _decks = new List<CharacterSelectCustomDeck>();
    [SerializeField] private CustomDeckCollection _collection;

    private void Start()
    {
        _collection.LoadDecks();
        LoadDecks();
    }

    private void LoadDecks()
    {
        foreach (var deck in _collection.Decks)
        {
            if (deck.Deck.Count > 0)
            {
                var deckUI = Instantiate(_deckPrefab, _deckList);
                deckUI.Deck = deck;
                deckUI.DeckName.text = deck.DeckName;
                _decks.Add(deckUI);
            }
        }
    }
}