using System;

public class MerchantUnit : Unit
{
    public static Action<UnitFaction> OnMerchantGainedGold;
    protected override void OnEnable()
    {
        base.OnEnable();
        GameStateMachine.OnStateChanged += GainMoney;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameStateMachine.OnStateChanged -= GainMoney;
    }

    private void GainMoney(GameState state)
    {
        if (state.GetType() == typeof(BuyUnitState) && state.Faction == Faction)
            if (_summoningSickness == false)
                OnMerchantGainedGold?.Invoke(Faction);
    }
}