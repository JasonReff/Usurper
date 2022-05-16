using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerDeck")]
public class PlayerDeck : ScriptableObject
{
    [SerializeField] private List<UnitData> _drawPile, _discardPile, _hand;
    [SerializeField] private UnitData _king;
    public UnitFaction Faction;

    public List<UnitData> Hand { get => _hand; }
    public UnitData King { get => _king; }

    public int DrawCount { get => _drawPile.Count; }
    public int DiscardCount { get => _discardPile.Count; }

    private void OnEnable()
    {
        StartGameState.OnGameStart += ShuffleHandAndDiscardIntoDrawPile;
    }

    private void OnDisable()
    {
        StartGameState.OnGameStart -= ShuffleHandAndDiscardIntoDrawPile;
    }

    private void ShuffleHandAndDiscardIntoDrawPile()
    {
        List<UnitData> handAndDiscard = new List<UnitData>();
        handAndDiscard.AddRange(_hand);
        _hand.Clear();
        handAndDiscard.AddRange(_discardPile);
        _discardPile.Clear();
        _drawPile.AddRange(handAndDiscard);
        var random = new System.Random();
        _drawPile = _drawPile.OrderBy(t => random.Next()).ToList();
    }

    public void DrawUnits()
    {
        var amount = 3;
        if (_drawPile.Count < 3)
            amount = _drawPile.Count;
        DrawXCards(amount);
        if (amount < 3 /*&& DiscardPile.Count > 0*/)
        {
            ReshuffleDeck();
            amount = 3 - amount;
            DrawXCards(amount);
        }

        void DrawXCards(int amount)
        {
            var cardsDrawn = _drawPile.Take(amount).ToList();
            foreach (var unit in cardsDrawn)
                _hand.Add(unit);
            foreach (var unit in cardsDrawn)
                _drawPile.Remove(unit);
        }
    }

    public void DiscardUnits()
    {
        foreach (var unit in _hand)
        {
            _discardPile.Add(unit);
        }
        _hand.Clear();
    }

    private void ReshuffleDeck()
    {
        foreach (var unit in _discardPile)
        {
            _drawPile.Add(unit);
        }
        _discardPile.Clear();
        var random = new System.Random();
        _drawPile = _drawPile.OrderBy(t => random.Next()).ToList();
    }

    public void ResetDeck(StartingDeck deck)
    {
        _drawPile.Clear();
        _hand.Clear();
        _discardPile.Clear();
        _drawPile.AddRange(deck.Deck);
        _king = deck.King;
    }
}
