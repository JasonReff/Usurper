using System.Collections;
using UnityEngine;

public class SinglePlayerGameCompleteManager : GameCompleteManager
{
    [SerializeField] private GameObject _roundWonPanel, _roundLostPanel, _runWonPanel, _bossDefeatedPanel;
    [SerializeField] private SinglePlayerStats _stats;
    protected override IEnumerator ResultScreenCoroutine(UnitFaction faction)
    {
        yield return new WaitForSeconds(2);
        _resultsCanvas.gameObject.SetActive(true);
        if (faction == UnitFaction.Player)
        {
            if (_stats.Round < 10)
            {
                if (_stats.Round % 3 != 0)
                    _roundWonPanel.SetActive(true);
                else
                    _bossDefeatedPanel.SetActive(true);
            }  
            else _runWonPanel.SetActive(true);
        }
        else
        {
            _roundLostPanel.SetActive(true);
        }
    }
}