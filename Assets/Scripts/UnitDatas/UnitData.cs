using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UnitData")]
public class UnitData : ScriptableObject
{
    public Unit Unit;
    public Sprite PlayerSprite, EnemySprite, Moveset;
    public int Cost;
    public string UnitName;
    public bool IsKing;

    public Sprite GetSprite(UnitFaction faction)
    {
        if (faction == UnitFaction.Player)
            return PlayerSprite;
        return EnemySprite;
    }

    public virtual bool CanMoveToTile<T>(IUnit unit, IBoardTile oldTile, IBoardTile newTile, IBoard<T> board) where T : IBoardTile
    {
        if (unit.SummoningSickness)
            return false;
        if (newTile.UnitOnTile != null && newTile.UnitOnTile?.Faction == unit.Faction)
            return false;
        return true;
    }

    protected Vector2 GetForwardVector(IUnit unit)
    {
        if (unit.Faction == UnitFaction.Player)
            return new Vector2(0, 1);
        else return new Vector2(0, -1);
    }

    protected List<IBoardTile> GetFurthestUnobstructedPaths<T>(IUnit unit, IBoardTile oldTile, IBoard<T> board, List<Vector2> directions) where T : IBoardTile
    {
        List<IBoardTile> tiles = new List<IBoardTile>();
        foreach (var direction in directions)
            tiles.AddRange(GetTilesInDirectionUntilObstructed(unit, oldTile, board, direction));
        return tiles;
    }

    protected List<IBoardTile> GetTilesInDirectionUntilObstructed<T>(IUnit unit, IBoardTile oldTile, IBoard<T> board, Vector2 direction) where T : IBoardTile
    {
        var tilePositions = new List<IBoardTile>();
        var nextTile = board.GetTileAtPosition(oldTile.TilePosition() + direction);
        if (DoesNextTileContainEnemy(unit, nextTile))
            tilePositions.Add(nextTile);
        while (nextTile != null && nextTile.UnitOnTile == null)
        {
            tilePositions.Add(nextTile);
            nextTile = board.GetTileAtPosition(nextTile.TilePosition() + direction);
            if (DoesNextTileContainEnemy(unit, nextTile))
            {
                tilePositions.Add(nextTile);
                break;
            }
        }
        return tilePositions;

        bool DoesNextTileContainEnemy(IUnit unit, IBoardTile tile)
        {
            if (tile != null && tile.UnitOnTile != null && tile.UnitOnTile.Faction != unit.Faction)
                return true;
            else return false;
        }
    }

    protected List<Vector2> DiagonalVectors()
    {
        var vectors = new List<Vector2>() { new Vector2(1, 1), new Vector2(-1, 1), new Vector2(1, -1), new Vector2(-1, -1) };
        return vectors;
    }

    protected List<Vector2> OrthogonalVectors()
    {
        var vectors = new List<Vector2>() { new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0) };
        return vectors;
    }

    protected bool IsForwardTile(IUnit unit, IBoardTile oldTile, IBoardTile newTile)
    {
        if (newTile.TilePosition() == oldTile.TilePosition() + GetForwardVector(unit))
            return true;
        else return false;
    }

    protected List<IBoardTile> GetForwardDiagonalTiles<T>(IUnit unit, IBoardTile oldTile, IBoard<T> board) where T : IBoardTile
    {
        var forwardVector = GetForwardVector(unit);
        var leftDiagonal = oldTile.TilePosition() + forwardVector + new Vector2(-1, 0);
        var rightDiagonal = oldTile.TilePosition() + forwardVector + new Vector2(1, 0);
        return new List<IBoardTile> { board.GetTileAtPosition(leftDiagonal), board.GetTileAtPosition(rightDiagonal) };
    }

    protected List<IBoardTile> GetNeighboringTiles<T>(IBoardTile oldTile, IBoard<T> board, List<Vector2> directions) where T : IBoardTile
    {
        var tiles = new List<IBoardTile>();
        foreach (var direction in directions)
        {
            var tile = board.GetTileAtPosition(oldTile.TilePosition() + direction);
            if (tile != null)
                tiles.Add(tile);
        }
        return tiles;
    }

    public List<Move> AllPossibleMoves<T>(IUnit unit, IBoardTile oldTile, IBoard<T> board) where T: IBoardTile
    {
        List<Move> moves = new List<Move>();
        foreach (var tile in board.TileArray)
        {
            var move = new Move(this, oldTile, tile);
            if (CanMoveToTile(unit, oldTile, tile, board))
                moves.Add(move);
        }
        return moves;
    }
}
