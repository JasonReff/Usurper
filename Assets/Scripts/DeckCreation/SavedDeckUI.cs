using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SavedDeckUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMP_InputField _nameTextbox;
    private StartingDeck _deck;

    public StartingDeck Deck { get => _deck; set {
            _deck = value;
        } }
    public static Action<SavedDeckUI> OnDeckClicked;
    public static Action OnDeckNameSaved;
    public static Action<SavedDeckUI> OnDeckCleared;

    private void OnEnable()
    {
        DeckPage.OnDeckSaved += SaveName;
    }

    private void OnDisable()
    {
        DeckPage.OnDeckSaved -= SaveName;
    }

    public void SaveName()
    {
        _deck.DeckName = _nameTextbox.text;
    }

    public void LoadName()
    {
        _nameTextbox.text = _deck.DeckName;
    }

    public void ClearDeck()
    {
        _deck.Deck.Clear();
        _deck.DeckName = null;
        OnDeckCleared?.Invoke(this);
        Destroy(gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnDeckClicked?.Invoke(this);
    }

    public void SelectDeck()
    {
        OnDeckClicked?.Invoke(this);
    }
}
