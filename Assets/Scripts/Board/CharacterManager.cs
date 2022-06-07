using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class CharacterManager : MonoBehaviourPunCallbacks
{

    [SerializeField] protected BoardTile _selectedTile;
    protected Unit _selectedUnit;
    [SerializeField] protected UnitFaction _faction;

    public UnitFaction Faction => _faction;

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

    public virtual void SelectTile(BoardTile tile)
    {
        if (tile.UnitOnTile != null && tile.UnitOnTile as Unit != _selectedUnit && tile.UnitOnTile.Faction == _faction)
        {
            _selectedTile = tile;
            _selectedUnit = tile.UnitOnTile as Unit;
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
        if (tile.UnitOnTile != null && tile.UnitOnTile.Faction == _faction)
        {
            _selectedUnit = tile.UnitOnTile as Unit;
            OnUnitSelected?.Invoke(_selectedUnit);
        }
        if (tile.UnitOnTile == null)
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
