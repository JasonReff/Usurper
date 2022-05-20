using System.Collections.Generic;
using UnityEngine;

public class KnightUnitData : UnitData
{
    public override bool CanMoveToTile<T>(IUnit unit, IBoardTile oldTile, IBoardTile tile, IBoard<T> board)
    {
        if (base.CanMoveToTile(unit, oldTile, tile, board) == false)
            return false;
        if (GetKnightTiles(oldTile, board).Contains(tile))
            return true;
        return false;
    }

    private List<IBoardTile> GetKnightTiles<T>(IBoardTile oldTile, IBoard<T> board) where T : IBoardTile
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
        List<IBoardTile> tiles = new List<IBoardTile>();
        foreach (var vector in vectors)
        {
            var tile = board.GetTileAtPosition(oldTile.TilePosition() + vector);
            if (tile != null)
                tiles.Add(tile);
        }
        return tiles;
    }
}