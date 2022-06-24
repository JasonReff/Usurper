using System.Collections;
using UnityEngine;

public class SinglePlayerGameCompleteManager : GameCompleteManager
{
    [SerializeField] private GameObject _roundWonPanel, _roundLostPanel, _runWonPanel, _bossDefeatedPanel;
    public GameObject ActivePanel;
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
                {
                    ActivePanel = _roundWonPanel;
                    _roundWonPanel.SetActive(true);
                    _roundWonPanel.GetComponent<SinglePlayerRoundEnd>().SetCards();
                }
                else
                {
                    ActivePanel = _bossDefeatedPanel;
                    _bossDefeatedPanel.SetActive(true);
                    _bossDefeatedPanel.GetComponent<UpgradeManagerUI>().SetUpgrades();
                }
            }
            else 
            {
                ActivePanel = _runWonPanel;
                _runWonPanel.SetActive(true);
                _runWonPanel.GetComponent<SinglePlayerRunEnd>().SetString();
            }
        }
        else
        {
            ActivePanel = _roundLostPanel;
            _roundLostPanel.SetActive(true);
        }
    }

    public void ResetActivePanel()
    {
        ActivePanel.SetActive(true);
    }
}