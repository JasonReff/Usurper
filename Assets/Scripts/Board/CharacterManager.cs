using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class CharacterManager : MonoBehaviour
{

    [SerializeField] protected BoardTile _selectedTile;
    protected Unit _selectedUnit;
    [SerializeField] protected UnitFaction _faction;

    public static Action<Unit> OnUnitSelected;

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
        if (tile.Unit != null && tile.Unit as Unit != _selectedUnit && tile.Unit.Faction == _faction)
        {
            _selectedTile = tile;
            _selectedUnit = tile.Unit as Unit;
            OnUnitSelected?.Invoke(_selectedUnit);
            return;
        }
        if (_selectedUnit != null && _selectedUnit.Faction == _faction)
        {
            _selectedUnit.MoveToTile(tile);
            _selectedUnit = null;
            return;
        }
        _selectedTile = tile;
        if (tile.Unit != null && tile.Unit.Faction == _faction)
        {
            _selectedUnit = tile.Unit as Unit;
            OnUnitSelected?.Invoke(_selectedUnit);
        }
        if (tile.Unit == null)
        {
            OnUnitSelected?.Invoke(null);
        }
            
    }

    protected bool IsGameStateCorrectFaction(GameState state)
    {
        return state.Faction == _faction;
    }

    public abstract void AddCharacterEvents(GameState state);
    public abstract void RemoveCharacterEvents(GameState state);
}
