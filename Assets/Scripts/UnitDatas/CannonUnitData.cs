using System.Collections.Generic;
using UnityEngine;

public class CannonUnitData : RangedUnitData
{
    public override bool CanMoveToTile<T>(IUnit unit, IBoardTile oldTile, IBoardTile newTile, IBoard<T> board)
    {
        if (base.CanMoveToTile(unit, oldTile, newTile, board) == false)
            return false;
        if (oldTile.IsTileAdjacent(newTile))
            return true;
        else if (oldTile.IsTileDiagonal(newTile))
            return true;
        return false;
    }

    public override List<IBoardTile> AttackableTiles<T>(IUnit unit, IBoardTile currentTile, IBoard<T> board)
    {
        var attackableTiles = new List<IBoardTile>();
        var tilesInRange = GetFurthestUnobstructedPaths(unit, currentTile, board, OrthogonalVectors());
        tilesInRange.RemoveAll(t => t.IsTileAdjacent(currentTile));
        foreach (var tile in tilesInRange)
            if (tile.UnitOnTile != null && tile.UnitOnTile.Faction != unit.Faction)
                attackableTiles.Add(tile);
        return attackableTiles;

    }
    public override List<Vector2> RangedTilePositions()
    {
        List<Vector2> positions = new List<Vector2>();
        List<Vector2> directions = new List<Vector2>
        {
            new Vector2(-1, 0),
            new Vector2(0, -1),
            new Vector2(1, 0),
            new Vector2(0, 1)
        };
        //change to first unobstructed tile
        for (int i = 2; i <= 8; i++)
        {
            foreach (var direction in directions)
                positions.Add(direction * i);
        }
        return positions;
    }
}