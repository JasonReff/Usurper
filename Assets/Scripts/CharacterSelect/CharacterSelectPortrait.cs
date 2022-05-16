using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSelectPortrait : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _kingIcon;
    [SerializeField] private UnitFaction _faction;
    [SerializeField] private PlayerDeck _playerDeck;
    [SerializeField] private TextMeshProUGUI _deckName;
    [SerializeField] private StartingDeck _equippedDeck;

    public StartingDeck EquippedDeck { get => _equippedDeck; }

    public static Action OnDeckEquipped;

    public void OnPointerEnter(PointerEventData eventData)
    {
        CharacterSelectDeck.OnDeckDropped += UpdateDeckAndKing;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CharacterSelectDeck.OnDeckDropped -= UpdateDeckAndKing;
    }

    private void Start()
    {
        _playerDeck.ResetDeck(_equippedDeck);
        _deckName.text = _equippedDeck.DeckName;
    }

    public void UpdateDeckAndKing(StartingDeck deck)
    {
        _equippedDeck = deck;
        if (_faction == UnitFaction.Player)
            _kingIcon.sprite = deck.King.PlayerSprite;
        else
            _kingIcon.sprite = deck.King.EnemySprite;
        _playerDeck.ResetDeck(deck);
        _deckName.text = deck.DeckName;
    }
}
