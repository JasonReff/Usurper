using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OvertimeManager : MonoBehaviour
{
    [SerializeField] private List<OvertimeTileDestroyer> _overtimes = new List<OvertimeTileDestroyer>();
    [SerializeField] private TextMeshProUGUI _nameTextbox, _turnsUntilDestructionTextbox;
    [SerializeField] private Image _screenCover;
    private OvertimeTileDestroyer _chosenOvertime;
    [SerializeField] private int _currentTurn = 0, _startingTurn = 80, _destroyDelay = 8, _turnsSinceOvertime;
    private float _textDisplayTime = 1f;

    private void OnEnable()
    {
        GameStateMachine.OnStateChanged += SetTurn;
    }

    private void OnDisable()
    {
        GameStateMachine.OnStateChanged -= SetTurn;
    }

    private void SetTurn(GameState state)
    {
        if (state.GetType() != typeof(WaitingState) && state.GetType() != typeof(StartGameState) && state.GetType() != typeof(PlayerWonState) && state.GetType() != typeof(PlayerLeftState))
        {
            _currentTurn++;
        }
        _turnsSinceOvertime = _currentTurn - _startingTurn;
        if (_currentTurn == _startingTurn)
        {
            ChooseOvertime();
            StartCoroutine(DisplayText());
        }
        else if (_chosenOvertime != null && _turnsSinceOvertime % _destroyDelay == 0)
        {
            OvertimeEffect();
        }
        if (_chosenOvertime != null && _turnsSinceOvertime % _chosenOvertime.ChooseDelay == 0)
            _chosenOvertime.ChooseTiles();
        if (_turnsSinceOvertime > 0)
            UpdateText();
    }

    private void UpdateText()
    {
        int turnsUntilDestruction = _destroyDelay - (_turnsSinceOvertime % _destroyDelay);
        _turnsUntilDestructionTextbox.text = $"Turns until tiles destroyed: {turnsUntilDestruction / 2}";
    }

    private void ChooseOvertime()
    {
        _chosenOvertime = _overtimes.Rand();
    }

    private IEnumerator DisplayText()
    {
        _screenCover.gameObject.SetActive(true);
        _nameTextbox.text = _chosenOvertime.OvertimeName;
        yield return new WaitForSeconds(_textDisplayTime);
        _screenCover.gameObject.SetActive(false);
        _nameTextbox.text = "";
        _chosenOvertime.ChooseTiles();
    }

    private void OvertimeEffect()
    {
        _chosenOvertime.DestroyTiles();
    }
}
