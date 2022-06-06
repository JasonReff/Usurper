using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StartingDeck")]
public class StartingDeck : ScriptableObject
{
    [SerializeField] private List<UnitData> _deck;
    [SerializeField] private UnitData _king;
    [SerializeField] private string _deckName;
    [SerializeField] private int _startingGold = 2, _goldPerTurn = 1;

    public List<UnitData> Deck { get => _deck; }
    public UnitData King { get => _king; set => _king = value; }
    public string DeckName { get => _deckName; set => _deckName = value; }
    public int StartingGold { get => _startingGold; set => _startingGold = value; }
    public int GoldPerTurn { get => _goldPerTurn; set => _goldPerTurn = value; }

    public void SetDeck(List<UnitData> units)
    {
        _deck = units;
    }

    public SerializableDeck Serialize()
    {
        var unitNames = new List<string>();
        var kingName = _king.UnitName;
        foreach (var unit in _deck)
            unitNames.Add(unit.UnitName);
        SerializableDeck deck = new SerializableDeck()
        {
            Units = unitNames,
            King = kingName,
            DeckName = _deckName
        };
        return deck;
    }

    public void Deserialize(SerializableDeck deck, CardPool cardPool)
    {
        var units = new List<UnitData>();
        foreach (var unitName in deck.Units)
            units.Add(cardPool.GetUnit(unitName));
        _king = cardPool.GetUnit(deck.King);
        _deckName = deck.DeckName;
        _deck = units;
    }
}

[System.Serializable]
public struct SerializableDeck
{
    public List<string> Units;
    public string King;
    public string DeckName;
}