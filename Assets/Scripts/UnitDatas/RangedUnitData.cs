using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class RangedUnitData : UnitData
{

    public bool IsRangedAttack<T>(IUnit unit, IBoardTile currentTile, IBoardTile newTile, IBoard<T> board) where T : IBoardTile
    {
        if(AttackableTiles(unit, currentTile, board).Contains(newTile))
        {
            return true;
        }
        return false;
    }

    public virtual List<IBoardTile> AttackableTiles<T>(IUnit unit, IBoardTile currentTile, IBoard<T> board) where T : IBoardTile
    {
        List<IBoardTile> tiles = TilesInRange(currentTile, board);
        var attackableTiles = new List<IBoardTile>();
        foreach (var tile in tiles)
        {
            var otherUnit = board.GetTileAtPosition(tile.TilePosition()).UnitOnTile;
            if (board is VirtualBoard)
                if ((board as VirtualBoard).VirtualUnits.Where(t => t.Tile.TilePosition() == tile.TilePosition()).Count() > 0)
                    otherUnit = (board as VirtualBoard).VirtualUnits.First(t => t.Tile.TilePosition() == tile.TilePosition());
            if (otherUnit != null && otherUnit.Faction != unit.Faction)
            {
                attackableTiles.Add(tile);
            }
        }
        return attackableTiles;
    }

    public List<IBoardTile> TilesInRange<T>(IBoardTile currentTile, IBoard<T> board) where T : IBoardTile
    {
        var tiles = new List<IBoardTile>();
        var currentPosition = currentTile.TilePosition();
        foreach (var position in RangedTilePositions())
        {
            if (board.GetTileAtPosition(currentPosition + position) != null)
                tiles.Add(board.GetTileAtPosition(currentPosition + position));
        }
        return tiles;
    }

    public virtual List<Vector2> RangedTilePositions()
    {
        return new List<Vector2>();
    }

    public override List<Move> AllPossibleMoves<T>(IUnit unit, IBoardTile oldTile, IBoard<T> board)
    {
        var moves = base.AllPossibleMoves(unit, oldTile, board);
        moves.RemoveAll(t => CanMoveToTile(unit, oldTile, t.NewTile, board) && t.NewTile.UnitOnTile?.Faction != unit.Faction);
        foreach (var tile in board.TileArray)
        {
            var move = new Move(this, oldTile, tile);
            if (IsRangedAttack(unit, oldTile, tile, board))
                moves.Add(move);
        }
        return moves;
    }
}

public interface IDontCaptureOnMovement
{

}