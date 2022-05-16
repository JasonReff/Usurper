public class RookUnit : Unit
{
    protected override bool CanMoveToTile(BoardTile tile)
    {
        if (base.CanMoveToTile(tile) == false)
            return false;
        if (GetFurthestUnobstructedPaths(OrthogonalVectors()).Contains(tile))
            return true;
        return false;
    }
}