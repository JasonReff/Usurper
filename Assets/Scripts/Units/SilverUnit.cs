using System.Collections.Generic;
using UnityEngine;

public class SilverUnit : Unit
{
    protected override bool CanMoveToTile(BoardTile tile)
    {
        if (base.CanMoveToTile(tile) == false)
            return false;
        if (GetNeighboringTiles(SilverDirections()).Contains(tile))
            return true;
        return false;
    }
    private List<Vector2> SilverDirections()
    {
        List<Vector2> directions = new List<Vector2>()
        {
            GetForwardVector() + new Vector2(-1, 0),
            GetForwardVector(),
            GetForwardVector() + new Vector2(1, 0),
            -GetForwardVector() + new Vector2(-1, 0),
            -GetForwardVector() + new Vector2(1, 0)
        };
        return directions;
    }
}