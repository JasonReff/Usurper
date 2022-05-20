using UnityEngine;
using UnityEngine.InputSystem;

public abstract class CharacterManager : MonoBehaviour
{

    [SerializeField] private BoardTile _selectedTile;
    private Unit _selectedUnit;
    [SerializeField] protected UnitFaction _faction;

    protected virtual void OnEnable()
    {
        GameStateMachine.OnStateChanged += AddCharacterEvents;
        GameStateMachine.OnStateChanged += RemoveCharacterEvents;
    }

    protected virtual void OnDisable()
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
            _selectedUnit = tile.Unit as Unit;
    }


    protected bool IsGameStateCorrectFaction(GameState state)
    {
        return state.Faction == _faction;
    }

    public abstract void AddCharacterEvents(GameState state);
    public abstract void RemoveCharacterEvents(GameState state);
}
