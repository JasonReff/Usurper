public class PlayerManager : CharacterManager
{
    public override void AddCharacterEvents(GameState state)
    {
        if (IsGameStateCorrectFaction(state) && state.GetType() == typeof(MoveUnitState))
            BoardTile.OnTileSelected += SelectTile;
    }

    public override void RemoveCharacterEvents(GameState state)
    {
        if (!IsGameStateCorrectFaction(state) || state.GetType() != typeof(MoveUnitState))
            BoardTile.OnTileSelected -= SelectTile;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        BoardVisualizer.OnVirtualBoardCreated += ClearSelection;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        BoardTile.OnTileSelected -= SelectTile;
        BoardVisualizer.OnVirtualBoardCreated -= ClearSelection;
    }

    private void ClearSelection()
    {
        _selectedUnit = null;
        _selectedTile = null;
    }
}
