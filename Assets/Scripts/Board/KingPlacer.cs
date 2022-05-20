using UnityEngine;

public class KingPlacer : MonoBehaviour
{
    [SerializeField] private PlayerDeck _deck;
    [SerializeField] private Vector2 _startingTile;

    private void Start()
    {
        var tile = Board.Instance.GetTileAtPosition(_startingTile);
        tile.PlaceUnit(_deck.King, _deck.Faction);
    }
}