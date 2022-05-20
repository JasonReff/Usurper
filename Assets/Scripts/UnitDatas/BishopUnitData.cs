public class BishopUnitData : UnitData
{
    public override bool CanMoveToTile<T>(IUnit unit, IBoardTile oldTile, IBoardTile tile, IBoard<T> board)
    {
        if (base.CanMoveToTile(unit, oldTile, tile, board) == false)
            return false;
        if (GetFurthestUnobstructedPaths(unit, oldTile, board, DiagonalVectors()).Contains(tile))
            return true;
        return false;
    }
}