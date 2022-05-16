using System.Collections.Generic;
using UnityEngine;

public class GoldUnit : Unit
{
    protected override bool CanMoveToTile(BoardTile tile)
    {
        if (base.CanMoveToTile(tile) == false)
            return false;
        if (GetNeighboringTiles(GoldDirections()).Contains(tile))
            return true;
        return false;
    }

    private List<Vector2> GoldDirections()
    {
        List<Vector2> directions = new List<Vector2>()
        {
            GetForwardVector() + new Vector2(-1, 0),
            GetForwardVector(),
            GetForwardVector() + new Vector2(1, 0),
            new Vector2(-1, 0),
            new Vector2(1, 0),
            -GetForwardVector()
        };
        return directions;
    }
}
