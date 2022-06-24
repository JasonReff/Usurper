using UnityEngine;

public class JesterUnit : Unit
{
    public override void MoveToTile<T>(T tile)
    {
        if (CanMoveToTile(tile as BoardTile))
        {
            var hoppedUnit = (UnitData as JesterUnitData).HoppedUnit(this, _tile, tile, Board.Instance);
            if (hoppedUnit != null)
            {
                (hoppedUnit as Unit).Tile.UnitOnTile = null;
                CaptureUnit(hoppedUnit as Unit);
            }
                
        }
        base.MoveToTile(tile);
    }
}