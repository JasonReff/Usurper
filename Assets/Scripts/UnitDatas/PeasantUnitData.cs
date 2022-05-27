public class PeasantUnitData : UnitData
{
    public override bool CanMoveToTile<T>(IUnit unit, IBoardTile oldTile, IBoardTile newTile, IBoard<T> board)
    {
        if (base.CanMoveToTile(unit, oldTile, newTile, board) == false)
            return false;
        if (GetForwardDiagonalTiles(unit, oldTile, board).Contains(newTile))
            return true;
        return false;
    }
}