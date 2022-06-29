using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerDeck")]
public class PlayerDeck : ScriptableObject
{
    private List<UnitData> _deckData;
    [SerializeField] private List<UnitCard> _drawPile = new List<UnitCard>(), _discardPile = new List<UnitCard>(), _hand = new List<UnitCard>();
    [SerializeField] private UnitData _king;
    [SerializeField] private int _startingGold;
    [SerializeField] private int _goldPerTurn;
    public UnitFaction Faction;

    public List<UnitCard> Hand { get => _hand; }
    public UnitData King { get => _king; set => _king = value; }

    public int DrawCount { get => _drawPile.Count; }
    public int DiscardCount { get => _discardPile.Count; }
    public int GoldPerTurn { get => _goldPerTurn; set => _goldPerTurn = value; }
    public int StartingGold { get => _startingGold; set => _startingGold = value; }
    public List<UnitCard> DrawPile { get => _drawPile; }
    public List<UnitCard> DiscardPile { get => _discardPile; }
    public List<UnitData> DeckData { get => _deckData; }

    private void OnEnable()
    {
        StartGameState.OnGameStart += ShuffleHandAndDiscardIntoDrawPile;
    }

    private void OnDisable()
    {
        StartGameState.OnGameStart -= ShuffleHandAndDiscardIntoDrawPile;
    }

    public void ShuffleHandAndDiscardIntoDrawPile()
    {
        List<UnitCard> handAndDiscard = new List<UnitCard>();
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
        if (amount < 3)
        {
            ReshuffleDeck();
            amount = 3 - amount;
            if (amount < _drawPile.Count)
            {
                DrawXCards(_drawPile.Count);
                return;
            }
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
        for (int i = _hand.Count - 1; i >= 0; i--)
        {
            UnitCard unit = _hand[i];
            if (unit.NumberOfUses <= 0)
                _hand.Remove(unit);
            if (unit.NumberOfUses > 0)
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
        _deckData = deck.Deck;
        _drawPile.AddRange(GetDeckCards());
        _king = deck.King;
        SetGold(deck.StartingGold, deck.GoldPerTurn);
    }

    public void ResetDeck(List<UnitData> units)
    {
        _drawPile.Clear();
        _hand.Clear();
        _discardPile.Clear();
        _deckData = units;
        _drawPile.AddRange(GetDeckCards());
    }

    public void AddCard(UnitData card)
    {
        _deckData.Add(card);
    }

    public void RemoveCard(UnitData card)
    {
        _deckData.Remove(card);
        ShuffleHandAndDiscardIntoDrawPile();
    }

    public List<UnitData> AllCards()
    {
        return _deckData;
    }

    public void SetGold(int startingGold, int goldPerTurn)
    {
        _startingGold = startingGold;
        _goldPerTurn = goldPerTurn;
    }

    public List<UnitCard> GetDeckCards()
    {
        var cards = new List<UnitCard>();
        foreach (var unitData in _deckData)
        {
            var card = new UnitCard(unitData);
            cards.Add(card);
        }
        return cards;
    }
}