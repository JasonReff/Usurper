using System.Collections.Generic;
using UnityEngine;

public class SilverUnitData : UnitData
{
    public override bool CanMoveToTile<T>(IUnit unit, IBoardTile oldTile, IBoardTile tile, IBoard<T> board)
    {
        if (base.CanMoveToTile(unit, oldTile, tile, board) == false)
            return false;
        if (GetNeighboringTiles(oldTile, board, SilverDirections(unit)).Contains(tile))
            return true;
        return false;
    }
    private List<Vector2> SilverDirections(IUnit unit)
    {
        List<Vector2> directions = new List<Vector2>()
        {
            GetForwardVector(unit) + new Vector2(-1, 0),
            GetForwardVector(unit),
            GetForwardVector(unit) + new Vector2(1, 0),
            -GetForwardVector(unit) + new Vector2(-1, 0),
            -GetForwardVector(unit) + new Vector2(1, 0)
        };
        return directions;
    }
}