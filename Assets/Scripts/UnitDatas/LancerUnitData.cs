using System.Collections.Generic;
using UnityEngine;

public class LancerUnitData : UnitData
{
    public override bool CanMoveToTile<T>(IUnit unit, IBoardTile oldTile, IBoardTile newTile, IBoard<T> board)
    {
        if (base.CanMoveToTile(unit, oldTile, newTile, board) == false)
            return false;
        if (GetFurthestUnobstructedPaths(unit, oldTile, board, LancerDirections(unit)).Contains(newTile))
            return true;
        if (GetNeighboringTiles(oldTile, board, LancerSpaces()).Contains(newTile))
            return true;
        return false;
    }

    private List<Vector2> LancerDirections(IUnit unit)
    {
        Vector2 forward = GetForwardVector(unit);
        return new List<Vector2>
        {
            new Vector2(-1, forward.y),
            new Vector2(0, forward.y),
            new Vector2(1, forward.y)
        };
    }

    private List<Vector2> LancerSpaces()
    {
        return new List<Vector2>
        {
            new Vector2(-1, 0),
            new Vector2(0, -1),
            new Vector2(1, 0)
        };
    }
}