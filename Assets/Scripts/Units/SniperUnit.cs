public class SniperUnit : RangedUnit
{
    public override bool CanMoveToTile(BoardTile tile)
    {
        if (_summoningSickness)
            return false;
        if (UnitData.CanMoveToTile(this, _tile, tile, Board.Instance) && tile.UnitOnTile != null && tile.UnitOnTile.Faction != Faction)
            return true;
        return base.CanMoveToTile(tile);
    }
}
