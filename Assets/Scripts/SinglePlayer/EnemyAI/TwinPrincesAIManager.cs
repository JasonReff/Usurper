public class TwinPrincesAIManager : SpecialEnemyManager
{
    private KingUnit _leftPrince, _rightPrince;

    public void SetKings(KingUnit leftPrince, KingUnit rightPrince)
    {
        _leftPrince = leftPrince;
        _rightPrince = rightPrince;
    }

    public override void ResetDeck()
    {
        base.ResetDeck();
        var deck = _specialDeck as MultipleKingDeck;
        GetComponent<TwinPrincesKingPlacer>().PlaceKings(deck.King, deck.King2);
    }

    private void SetActiveKing()
    {
        var random = UnityEngine.Random.Range(0, 2);
        if (random == 0)
            _kingUnit = _leftPrince;
        else _kingUnit = _rightPrince;
        Shop.SetKing(_kingUnit);
    }

    public override void AddCharacterEvents(GameState state)
    {
        if (state.Faction == UnitFaction.Enemy && state.GetType() == typeof(BuyUnitState))
            SetActiveKing();
        base.AddCharacterEvents(state);
    }
}
