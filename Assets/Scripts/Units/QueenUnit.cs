public class QueenUnit : Unit
{
    protected override bool CanMoveToTile(BoardTile tile)
    {
        if (base.CanMoveToTile(tile) == false)
            return false;
        if (GetFurthestUnobstructedPaths(DiagonalVectors()).Contains(tile))
            return true;
        if (GetFurthestUnobstructedPaths(OrthogonalVectors()).Contains(tile))
            return true;
        return false;
    }
}