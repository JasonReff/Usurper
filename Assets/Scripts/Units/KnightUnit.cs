using System.Collections.Generic;
using UnityEngine;

public class KnightUnit : Unit
{
    protected override bool CanMoveToTile(BoardTile tile)
    {
        if (base.CanMoveToTile(tile) == false)
            return false;
        if (GetKnightTiles().Contains(tile))
            return true;
        return false;
    }

    private List<BoardTile> GetKnightTiles()
    {
        List<Vector2> vectors = new List<Vector2>()
        {
            new Vector2(1, 2),
            new Vector2(2, 1),
            new Vector2(-1, 2),
            new Vector2(-2, 1),
            new Vector2(1, -2),
            new Vector2(2, -1),
            new Vector2(-1, -2),
            new Vector2(-2, -1),
        };
        List<BoardTile> tiles = new List<BoardTile>();
        foreach (var vector in vectors)
        {
            var tile = Board.GetTileAtPosition((Vector2)_tile.transform.localPosition + vector);
            if (tile != null)
                tiles.Add(tile);
        }
        return tiles;
    }
}