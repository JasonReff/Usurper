public class PawnUnitData : UnitData
{
    public override bool CanMoveToTile<T>(IUnit unit, IBoardTile oldTile, IBoardTile newTile, IBoard<T> board)
    {
        if (base.CanMoveToTile(unit, oldTile, newTile, board) == false)
            return false;
        if (GetForwardDiagonalTiles(unit, oldTile, board).Contains(newTile) && newTile.Unit != null && newTile.Unit.Faction != unit.Faction)
            return true;
        if (IsForwardTile(unit, oldTile, newTile) && newTile.Unit == null)
            return true;
        return false;
    }
}