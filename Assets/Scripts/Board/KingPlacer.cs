﻿using UnityEngine;

public class KingPlacer : MonoBehaviour
{
    [SerializeField] private PlayerDeck _deck;
    [SerializeField] protected Vector2 _startingTile;
    protected KingUnit _king;

    public virtual void PlaceKing()
    {
        AdjustTilePosition();
        var tile = Board.Instance.GetTileAtPosition(_startingTile);
        tile.PlaceUnit(_deck.King, _deck.Faction);
    }

    private void AdjustTilePosition()
    {
        CharacterManager manager = GetComponent<CharacterManager>();
        if (manager.Faction == UnitFaction.Player)
        {
            _startingTile = new Vector2(0, -4);
        }
        else _startingTile = new Vector2(0, 3);
    }

    public virtual void Start()
    {
        PlaceKing();
    }
}
