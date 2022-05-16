public class ShogiPawnUnit : Unit
{
    protected override bool CanMoveToTile(BoardTile tile)
    {
        if (base.CanMoveToTile(tile) == false)
            return false;
        if (IsForwardTile(tile))
            return true;
        return false;
    }
}