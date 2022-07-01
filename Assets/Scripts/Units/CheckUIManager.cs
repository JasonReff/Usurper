using TMPro;
using UnityEngine;

public class CheckUIManager : MonoBehaviour
{
    [SerializeField] private GameplaySettings _settings;
    [SerializeField] private TextMeshProUGUI _checkTextbox;

    private void OnEnable()
    {
        GameStateMachine.OnStateChanged += LookForChecks;
        OnlineGameStateMachine.OnOnlineStateChanged += LookForChecks;
    }

    private void OnDisable()
    {
        GameStateMachine.OnStateChanged -= LookForChecks;
        OnlineGameStateMachine.OnOnlineStateChanged -= LookForChecks;
    }

    private void LookForChecks(GameState state)
    {
        if (!_settings.DisplayChecks)
            return;
        var currentBoard = new VirtualBoard(Board.Instance);
        foreach (var unit in currentBoard.VirtualUnits)
            unit.SummoningSickness = false;
        currentBoard.CalculateEvaluation();
        DisplayChecks(currentBoard.KingInCheck);
    }

    private void LookForChecks()
    {
        if (!_settings.DisplayChecks)
            return;
        var currentBoard = new VirtualBoard(Board.Instance);
        foreach (var unit in currentBoard.VirtualUnits)
            unit.SummoningSickness = false;
        currentBoard.CalculateEvaluation();
        DisplayChecks(currentBoard.KingInCheck);
    }

    private void DisplayChecks(bool isKingInCheck)
    {
        if (isKingInCheck)
        {
            _checkTextbox.enabled = true;
        }
        else
        {
            _checkTextbox.enabled = false;
        }
    }
}