using System.Collections.Generic;
using UnityEngine;

public class JesterUnitData : UnitData, IHopPieces
{

    public override bool CanMoveToTile<T>(IUnit unit, IBoardTile oldTile, IBoardTile newTile, IBoard<T> board)
    {
        if (base.CanMoveToTile(unit, oldTile, newTile, board) == false)
            return false;
        if (newTile.UnitOnTile != null)
            return false;
        if (GetNeighboringTiles(oldTile, board, JesterPositions()).Contains(newTile))
            return true;
        return false;
    }

    private List<Vector2> JesterPositions()
    {
        var positions = new List<Vector2>()
        {
            new Vector2(2, 2),
            new Vector2(-2, 2),
            new Vector2(2, -2),
            new Vector2(-2, -2)
        };
        return positions;
    }

    public IUnit HoppedUnit<T>(IUnit unit, IBoardTile oldTile, IBoardTile newTile, IBoard<T> board) where T : IBoardTile
    {
        var hoppedTilePosition = new Vector2((oldTile.TilePosition().x + newTile.TilePosition().x) / 2, (oldTile.TilePosition().y + newTile.TilePosition().y) / 2);
        var hoppedTile = board.GetTileAtPosition(hoppedTilePosition);
        var hoppedUnit = hoppedTile.UnitOnTile;
        if (hoppedUnit != null && hoppedUnit.Faction != unit.Faction)
            return hoppedUnit;
        return null;
    }

    public List<Vector2> HopSpaces()
    {
        return new List<Vector2>()
        {
            new Vector2(-1, -1),
            new Vector2(1, -1),
            new Vector2(1, 1),
            new Vector2(-1, 1)
        };
    }
}

public interface IHopPieces
{
    public IUnit HoppedUnit<T>(IUnit unit, IBoardTile oldTile, IBoardTile newTile, IBoard<T> board) where T : IBoardTile;

    public List<Vector2> HopSpaces();
}