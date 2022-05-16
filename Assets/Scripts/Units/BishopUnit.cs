public class BishopUnit : Unit
{
    protected override bool CanMoveToTile(BoardTile tile)
    {
        if (base.CanMoveToTile(tile) == false)
            return false;
        if (GetFurthestUnobstructedPaths(DiagonalVectors()).Contains(tile))
            return true;
        return false;
    }

}
