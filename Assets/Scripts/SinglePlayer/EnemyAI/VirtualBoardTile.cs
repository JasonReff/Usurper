using UnityEngine;

public class VirtualBoardTile : IBoardTile
{
    public VirtualUnit _unit;
    public Vector2 _tilePosition;

    public VirtualBoardTile(Vector2 position)
    {
        _tilePosition = position;
    }

    public VirtualBoardTile(VirtualBoardTile tile, VirtualUnit unit = null)
    {
        _tilePosition = tile._tilePosition;
        _unit = unit;
    }

    public IUnit UnitOnTile { get => _unit; set => _unit = value as VirtualUnit; }

    public bool IsTileAdjacent(IBoardTile otherTile)
    {
        int distance = (int)(Mathf.Abs(otherTile.TilePosition().x - _tilePosition.x) + Mathf.Abs(otherTile.TilePosition().y - _tilePosition.y));
        if (distance == 1)
        {
            return true;
        }
        return false;
    }

    public bool IsTileDiagonal(IBoardTile otherTile)
    {
        Vector2 difference = otherTile.TilePosition() - _tilePosition;
        if (Mathf.Abs(difference.x) == Mathf.Abs(difference.y) && Mathf.Abs(difference.x) == 1)
            return true;
        else return false;
    }

    public Vector2 TilePosition()
    {
        return _tilePosition;
    }
}