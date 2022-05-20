using System.Collections.Generic;

public class KingUnitData : UnitData
{
    public override bool CanMoveToTile<T>(IUnit unit, IBoardTile oldTile, IBoardTile newTile, IBoard<T> board)
    {
        if (base.CanMoveToTile(unit, oldTile, newTile, board) == false)
            return false;
        if (oldTile.IsTileAdjacent(newTile))
            return true;
        else if (oldTile.IsTileDiagonal(newTile))
            return true;
        return false;
    }

    public List<UnitPlacement> GetPossibleUnitPlacements(IUnit unit, VirtualBoardTile currentTile, VirtualBoard currentBoard)
    {
        List<UnitPlacement> placements = new List<UnitPlacement>();
        List<VirtualBoardTile> openSpaces = new List<VirtualBoardTile>();
        foreach (var tile in currentBoard.TileArray)
            if (CanMoveToTile(Unit, currentTile, tile, currentBoard))
                openSpaces.Add(tile);
        foreach (var tile in openSpaces)
        {
            placements.Add(new UnitPlacement(unit, tile));
        }
        return placements;
    }
}