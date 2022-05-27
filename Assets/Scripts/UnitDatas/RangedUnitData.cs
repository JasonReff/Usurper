using System.Collections.Generic;
using UnityEngine;

public abstract class RangedUnitData : UnitData
{

    public bool IsRangedAttack<T>(IUnit unit, IBoardTile currentTile, IBoardTile newTile, IBoard<T> board) where T : IBoardTile
    {
        if(AttackableTiles(unit, currentTile, board).Contains(newTile))
        {
            return true;
        }
        return false;
    }

    public List<IBoardTile> AttackableTiles<T>(IUnit unit, IBoardTile currentTile, IBoard<T> board) where T : IBoardTile
    {
        List<IBoardTile> tiles = TilesInRange(currentTile, board);
        var attackableTiles = new List<IBoardTile>();
        foreach (var tile in tiles)
            if (tile.Unit != null && tile.Unit.Faction != unit.Faction)
            {
                attackableTiles.Add(tile);
            }
        return attackableTiles;
    }

    public List<IBoardTile> TilesInRange<T>(IBoardTile currentTile, IBoard<T> board) where T : IBoardTile
    {
        var tiles = new List<IBoardTile>();
        var currentPosition = currentTile.TilePosition();
        foreach (var position in RangedTilePositions())
        {
            if (board.GetTileAtPosition(currentPosition + position) != null)
                tiles.Add(board.GetTileAtPosition(currentPosition + position));
        }
        return tiles;
    }

    public abstract List<Vector2> RangedTilePositions();
}
