using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameCompleteManager : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _resultsText;
    [SerializeField] protected GameObject _resultsCanvas;

    private void OnEnable()
    {
        GameStateMachine.OnStateChanged += OnGameComplete;
    }

    private void OnDisable()
    {
        GameStateMachine.OnStateChanged -= OnGameComplete;
    }

    private void OnGameComplete(GameState state)
    {
        if (state.GetType() == typeof(PlayerWonState))
        {
            StartCoroutine(ResultScreenCoroutine(state.Faction));
        }
        else if (state.GetType() == typeof(PlayerLeftState))
            OpponentDisconnected();
    }

    protected virtual IEnumerator ResultScreenCoroutine(UnitFaction faction)
    {
        yield return new WaitForSeconds(2);
        _resultsCanvas.gameObject.SetActive(true);
        _resultsText.text = $"Player {faction} won!";
    }

    private void OpponentDisconnected()
    {
        _resultsCanvas.gameObject.SetActive(true);
        _resultsText.text = "Opponent Disconnected";
    }
}
