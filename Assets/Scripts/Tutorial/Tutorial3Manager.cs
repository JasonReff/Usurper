using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tutorial3Manager : TutorialManager
{
    [SerializeField] private GameObject _sectionFailedPanel;

    private void OnEnable()
    {
        GameStateMachine.OnStateChanged += CheckForEnemyKing;
    }

    private void OnDisable()
    {
        GameStateMachine.OnStateChanged -= CheckForEnemyKing;
    }

    private void CheckForEnemyKing(GameState state)
    {
        if (state is MoveUnitState)
            return;
        if (Board.Instance.EnemyUnits.Any(t => t.UnitData.IsKing))
        {
            SectionFailed();
        }
        else TutorialComplete();
    }

    private void SectionFailed()
    {
        StartCoroutine(FailedCoroutine());

        IEnumerator FailedCoroutine()
        {
            yield return new WaitForSeconds(1f);
            _sectionFailedPanel.SetActive(true);
            _backgroundCover.SetActive(true);
        }
        
    }

    public override void StartTutorial()
    {
        GameStateMachine.Instance.ChangeState(new MoveUnitState(GameStateMachine.Instance, UnitFaction.Player));
    }
}
