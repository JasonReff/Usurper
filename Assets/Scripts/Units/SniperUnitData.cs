using System.Collections.Generic;

public class SniperUnitData : RangedUnitData
{
    public override bool CanMoveToTile<T>(IUnit unit, IBoardTile oldTile, IBoardTile newTile, IBoard<T> board)
    {
        if (base.CanMoveToTile(unit, oldTile, newTile, board) == false)
            return false;
        else if (oldTile.IsTileDiagonal(newTile))
            return true;
        return false;
    }

    public override List<IBoardTile> AttackableTiles<T>(IUnit unit, IBoardTile currentTile, IBoard<T> board)
    {
        var attackableTiles = new List<IBoardTile>();
        var tilesInRange = GetFurthestUnobstructedPaths(unit, currentTile, board, new List<UnityEngine.Vector2>() { GetForwardVector(unit)});
        tilesInRange.RemoveAll(t => t.IsTileAdjacent(currentTile));
        foreach (var tile in tilesInRange)
            if (tile.UnitOnTile != null && tile.UnitOnTile.Faction != unit.Faction)
                attackableTiles.Add(tile);
        return attackableTiles;
    }
}