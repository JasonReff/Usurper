using UnityEngine;
using UnityEngine.InputSystem;

public abstract class CharacterManager : MonoBehaviour
{

    [SerializeField] private BoardTile _selectedTile;
    private Unit _selectedUnit;
    [SerializeField] private UnitFaction _faction;

    private void OnEnable()
    {
        GameStateMachine.OnStateChanged += AddCharacterEvents;
        GameStateMachine.OnStateChanged += RemoveCharacterEvents;
    }

    private void OnDisable()
    {
        GameStateMachine.OnStateChanged -= AddCharacterEvents;
        GameStateMachine.OnStateChanged -= RemoveCharacterEvents;
    }

    public void SelectTile(BoardTile tile)
    {
        if (_selectedUnit != null && _selectedUnit.Faction == _faction)
        {
            _selectedUnit.MoveToTile(tile);
            _selectedUnit = null;
            return;
        }
        _selectedTile = tile;
        if (tile.Unit != null)
            _selectedUnit = tile.Unit;
    }


    protected bool IsGameStateCorrectFaction(GameState state)
    {
        return state.Faction == _faction;
    }

    public abstract void AddCharacterEvents(GameState state);
    public abstract void RemoveCharacterEvents(GameState state);
}
