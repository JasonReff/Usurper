using System;
using System.Collections;
using System.Linq;

public class KingUnit : Unit
{
    public static event Action<UnitFaction> OnMoneyGenerated;
    public static event Action<KingUnit> OnKingCaptured;
    protected override bool CanMoveToTile(BoardTile tile)
    {
        if (base.CanMoveToTile(tile) == false)
            return false;
        if (_tile.IsTileAdjacent(tile))
            return true;
        else if (_tile.IsTileDiagonal(tile))
            return true;
        return false;
    }

    protected override IEnumerator UnitDeath()
    {
        UnitFaction oppositeFaction = UnitFaction.Player;
        if (Faction == UnitFaction.Player)
            oppositeFaction = UnitFaction.Enemy;
        GameStateMachine.Instance.ChangeState(new PlayerWonState(GameStateMachine.Instance, oppositeFaction));
        return base.UnitDeath();
        
    }

    private void OnEnable()
    {
        GameStateMachine.OnStateChanged += GenerateMoney;
    }

    private void OnDisable()
    {
        GameStateMachine.OnStateChanged -= GenerateMoney;
    }

    private void GenerateMoney(GameState state)
    {
        if (state.Faction == Faction && state.GetType() == typeof(MoveUnitState))
            OnMoneyGenerated?.Invoke(Faction);
    }
}
