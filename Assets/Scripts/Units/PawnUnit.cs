public class PawnUnit : Unit
{
    protected override bool CanMoveToTile(BoardTile tile)
    {
        if (base.CanMoveToTile(tile) == false)
            return false;
        if (GetForwardDiagonalTiles().Contains(tile) && tile.Unit != null && tile.Unit.Faction != Faction)
            return true;
        if (IsForwardTile(tile) && tile.Unit == null)
            return true;
        return false;
    }
}
