using Photon.Pun;
using TMPro;
using UnityEngine;

public class CheckUIManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameplaySettings _settings;
    [SerializeField] private TextMeshProUGUI _checkTextbox;

    protected virtual void OnEnable()
    {
        GameStateMachine.AfterStateChanged += LookForChecks;
    }

    protected virtual void OnDisable()
    {
        GameStateMachine.AfterStateChanged -= LookForChecks;
    }

    protected virtual void LookForChecks()
    {
        if (!_settings.DisplayChecks)
            return;
        var currentBoard = new VirtualBoard(Board.Instance);
        foreach (var unit in currentBoard.VirtualUnits)
            unit.SummoningSickness = false;
        currentBoard.CalculateEvaluation();
        DisplayChecks(currentBoard.KingInCheck);
    }

    protected virtual void DisplayChecks(bool isKingInCheck)
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
