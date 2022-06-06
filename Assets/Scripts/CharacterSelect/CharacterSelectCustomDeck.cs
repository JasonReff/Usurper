using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterSelectCustomDeck : CharacterSelectDeck
{
    [SerializeField] private TextMeshProUGUI _deckName;

    public TextMeshProUGUI DeckName { get => _deckName; }
}