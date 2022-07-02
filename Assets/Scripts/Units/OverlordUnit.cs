using System;
using System.Collections;
using System.Collections.Generic;

public class OverlordUnit : KingUnit
{
    public static Action<OverlordUnit> OnOverlordKilled;
    protected override IEnumerator UnitDeath()
    {
        DestroyAdjacentUnits();
        OnOverlordKilled?.Invoke(this);
        return base.UnitDeath();
    }

    private void DestroyAdjacentUnits()
    {
        var units = new List<Unit>();
        foreach (var adjacentTile in UnitData.GetNeighboringTiles(Tile, Board.Instance, UnitData.AllAdjacentVectors()))
            if (adjacentTile.UnitOnTile != null)
                units.Add(adjacentTile.UnitOnTile as Unit);
        foreach (var unit in units)
            CaptureUnit(unit);
    }
}