using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StartingDeck")]
public class StartingDeck : ScriptableObject
{
    [SerializeField] private List<UnitData> _deck;
    [SerializeField] private UnitData _king;
    [SerializeField] private string _deckName;

    public List<UnitData> Deck { get => _deck; }
    public UnitData King { get => _king; }
    public string DeckName { get => _deckName; }
}