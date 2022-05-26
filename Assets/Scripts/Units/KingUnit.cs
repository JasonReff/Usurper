using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class KingUnit : Unit
{
    public static event Action<UnitFaction> OnMoneyGenerated;
    public static event Action<KingUnit> OnKingCaptured;

    protected override IEnumerator UnitDeath()
    {
        UnitFaction oppositeFaction = UnitFaction.Player;
        if (Faction == UnitFaction.Player)
            oppositeFaction = UnitFaction.Enemy;
        GameStateMachine.Instance.ChangeState(new PlayerWonState(GameStateMachine.Instance, oppositeFaction));
        return base.UnitDeath();
    }

    public List<UnitPlacement> GetPossibleUnitPlacements(Unit unit)
    {
        List<UnitPlacement> placements = new List<UnitPlacement>();
        List<BoardTile> openSpaces = new List<BoardTile>();
        foreach (var tile in Board.Instance.TileArray)
            if (CanMoveToTile(tile))
                openSpaces.Add(tile);
        foreach (var tile in openSpaces)
        {
            placements.Add(new UnitPlacement(unit, tile));
        }
        return placements;
    }
}
