using System.Collections.Generic;
using UnityEngine;

public class KingUnitData : UnitData
{
    [SerializeField] private List<Vector2> _additionalMovementTiles = new List<Vector2>();
    [SerializeField] private List<Vector2> _additionalPlacementTiles = new List<Vector2>();
    public void AddAdditionalMovement(List<Vector2> newTiles)
    {
        _additionalMovementTiles.AddRange(newTiles);
    }

    public void AddAdditionalPlacement(List<Vector2> newTiles)
    {
        _additionalPlacementTiles.AddRange(newTiles);
    }

    public void ClearAdditionalMovementSpaces()
    {
        _additionalMovementTiles.Clear();
    }

    public void ClearAdditionalPlacementSpaces()
    {
        _additionalPlacementTiles.Clear();
    }

    public override bool CanMoveToTile<T>(IUnit unit, IBoardTile oldTile, IBoardTile newTile, IBoard<T> board)
    {
        if (base.CanMoveToTile(unit, oldTile, newTile, board) == false)
            return false;
        if (IsTileSurroundingKing(oldTile, newTile))
            return true;
        if (unit.Faction == UnitFaction.Player && GetNeighboringTiles(oldTile, board, _additionalMovementTiles).Contains(newTile))
            return true;
        return false;
    }

    private bool IsTileSurroundingKing(IBoardTile oldTile, IBoardTile newTile)
    {
        if (oldTile.IsTileAdjacent(newTile))
            return true;
        else if (oldTile.IsTileDiagonal(newTile))
            return true;
        else return false;
    }

    public bool IsPlacementTile<T>(IUnit unit, IBoardTile oldTile, IBoardTile newTile, IBoard<T> board) where T : IBoardTile
    {
        if (IsTileSurroundingKing(oldTile, newTile))
            return true;
        else if (unit.Faction == UnitFaction.Player && GetNeighboringTiles(oldTile, board, _additionalPlacementTiles).Contains(newTile))
            return true;
        return false;
    }

    public List<UnitPlacement> GetVirtualKingUnitPlacements(IUnit unit, VirtualBoardTile currentTile, VirtualBoard currentBoard)
    {
        List<UnitPlacement> placements = new List<UnitPlacement>();
        List<VirtualBoardTile> openSpaces = new List<VirtualBoardTile>();
        foreach (var tile in currentBoard.TileArray)
            if (IsTileSurroundingKing(tile, currentTile))
                openSpaces.Add(tile);
        foreach (var tile in openSpaces)
        {
            placements.Add(new UnitPlacement(unit, tile));
        }
        return placements;
    }
}
