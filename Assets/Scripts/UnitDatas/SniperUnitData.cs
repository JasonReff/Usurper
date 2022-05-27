using System.Collections.Generic;
using UnityEngine;

public class SniperUnitData : RangedUnitData
{
    public override bool CanMoveToTile<T>(IUnit unit, IBoardTile oldTile, IBoardTile newTile, IBoard<T> board)
    {
        if (base.CanMoveToTile(unit, oldTile, newTile, board) == false)
            return false;
        if (GetNeighboringTiles(oldTile, board, MovementPositions()).Contains(newTile))
            return true;
        return false;
    }
    private List<Vector2> MovementPositions()
    {
        return new List<Vector2>
        {
            new Vector2(1, 1),
            new Vector2(1, -1),
            new Vector2(-1, 1),
            new Vector2(-1, -1)
        };
    }

    public override List<Vector2> RangedTilePositions()
    {
        return new List<Vector2>
        {
            new Vector2(2, 2),
            new Vector2(-2, 2),
            new Vector2(2, -2),
            new Vector2(-2, -2)
        };
    }
}