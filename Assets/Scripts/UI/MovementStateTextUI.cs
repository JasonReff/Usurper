using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MovementStateTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textbox;
    private bool _isMovementTurn;

    private void OnEnable()
    {
        MoveUnitState.OnMoveStateBegan += OnMoveStateStarted;
        MoveUnitState.OnMoveStateEnded += OnMoveStateEnded;
        BoardVisualizer.OnVirtualBoardCreated += HideUI;
        BoardVisualizer.OnBoardHidden += ShowUI;
    }

    private void OnDisable()
    {
        MoveUnitState.OnMoveStateBegan -= OnMoveStateStarted;
        MoveUnitState.OnMoveStateEnded -= OnMoveStateEnded;
        BoardVisualizer.OnVirtualBoardCreated -= HideUI;
        BoardVisualizer.OnBoardHidden -= ShowUI;
    }

    private void OnMoveStateStarted(GameState state)
    {
        ShowUI(state.Faction);
    }

    private void OnMoveStateEnded()
    {
        HideUI();
    }

    private void ShowUI(UnitFaction faction)
    {
        _textbox.enabled = true;
        _textbox.text = faction.ToString() + " Move Piece";
    }

    private void HideUI()
    {
        if (_textbox.enabled)
            _isMovementTurn = true;
        else _isMovementTurn = false;
        _textbox.enabled = false;
    }

    private void ShowUI()
    {
        if (_isMovementTurn)
            _textbox.enabled = true;
    }
}
