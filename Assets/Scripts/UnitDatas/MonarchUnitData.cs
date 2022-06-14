public class MonarchUnitData : KingUnitData
{
    public override bool CanMoveToTile<T>(IUnit unit, IBoardTile oldTile, IBoardTile newTile, IBoard<T> board)
    {
        if (base.CanMoveToTile(unit, oldTile, newTile, board) == false)
            return false;
        if (GetFurthestUnobstructedPaths(unit, oldTile, board, DiagonalVectors()).Contains(newTile))
            return true;
        if (GetFurthestUnobstructedPaths(unit, oldTile, board, OrthogonalVectors()).Contains(newTile))
            return true;
        return false;
    }
}