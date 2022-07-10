public class TwinPrincesAIManager : SpecialEnemyManager
{
    private KingUnit _leftPrince, _rightPrince;

    protected override void OnEnable()
    {
        base.OnEnable();
        KingUnit.OnKingCaptured += OnKingCaptured;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        KingUnit.OnKingCaptured -= OnKingCaptured;
    }

    private void OnKingCaptured(KingUnit unit)
    {
        if (unit == _leftPrince)
            _leftPrince = null;
        else if (unit == _rightPrince)
            _rightPrince = null;
    }

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
        if (_leftPrince == null)
            _kingUnit = _rightPrince;
        else if (_rightPrince == null)
            _kingUnit = _leftPrince;
        var random = UnityEngine.Random.Range(0, 2);
        if (random == 0)
            _kingUnit = _leftPrince;
        else _kingUnit = _rightPrince;
        Shop.SetKing(_kingUnit);
    }

    public override void AddCharacterEvents(GameState state)
    {
        if (state.Faction == UnitFaction.Black && state.GetType() == typeof(BuyUnitState))
            SetActiveKing();
        base.AddCharacterEvents(state);
    }
}
