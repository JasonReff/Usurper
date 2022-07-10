using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class TutorialManager : MonoBehaviour
{
    [SerializeField] protected GameObject _tutorialCompletePanel;
    [SerializeField] private List<GameObject> _tutorialPanels;

    public abstract void StartTutorial();

    protected void TutorialComplete()
    {
        GameStateMachine.Instance.ChangeState(new PlayerWonState(GameStateMachine.Instance, UnitFaction.White));
        StartCoroutine(TutorialCompleteCoroutine());

        IEnumerator TutorialCompleteCoroutine()
        {
            yield return new WaitForSeconds(1f);
            HideTutorialPanels();
            _tutorialCompletePanel.SetActive(true);
        }
        
    }

    protected void HideTutorialPanels() 
    { 
        foreach (var panel in _tutorialPanels)
        {
            panel.SetActive(false);
        }
    }
}
