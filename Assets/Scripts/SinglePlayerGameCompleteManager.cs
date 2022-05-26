using System.Collections;
using UnityEngine;

public class SinglePlayerGameCompleteManager : GameCompleteManager
{
    [SerializeField] private GameObject _roundWonPanel, _roundLostPanel;
    protected override IEnumerator ResultScreenCoroutine(UnitFaction faction)
    {
        yield return new WaitForSeconds(2);
        _resultsCanvas.gameObject.SetActive(true);
        if (faction == UnitFaction.Player)
        {
            _roundWonPanel.SetActive(true);
        }
        else
        {
            _roundLostPanel.SetActive(true);
        }
    }
}