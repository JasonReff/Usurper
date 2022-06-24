using System.Collections.Generic;
using UnityEngine;

public class MonarchUnitData : KingUnitData
{
    public override bool CanMoveToTile<T>(IUnit unit, IBoardTile oldTile, IBoardTile newTile, IBoard<T> board)
    {
        if (base.CanMoveToTile(unit, oldTile, newTile, board) == true)
            return true;
        if (GetFurthestUnobstructedPaths(unit, oldTile, board, AllAdjacentVectors()).Contains(newTile) && GetNeighboringTiles(oldTile, board, MonarchPositions()).Contains(newTile))
            return true;
        return false;
    }

    private List<Vector2> MonarchPositions()
    {
        var positions = new List<Vector2>()
        {
            new Vector2(0, 2),
            new Vector2(-2, 0),
            new Vector2(0, -2),
            new Vector2(2, 0),
            new Vector2(-2, -2),
            new Vector2(2, 2),
            new Vector2(-2, 2),
            new Vector2(2, -2)
        };
        return positions;
    }
}