public class EnemyAIManager : CharacterManager
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
}