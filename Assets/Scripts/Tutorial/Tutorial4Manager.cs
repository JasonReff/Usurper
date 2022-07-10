using System.Collections;
using System.Linq;
using UnityEngine;

public class Tutorial4Manager : TutorialManager
{
    [SerializeField] private GameObject _sectionFailedPanel;
    [SerializeField] private PlayerDeck _tutorialDeck;
    [SerializeField] private StartingDeck _tutorialStartingDeck;
    [SerializeField] private GameObject _nextPanel, _previousPanel;

    private void OnEnable()
    {
        GameStateMachine.OnStateChanged += IsKingCaptured;
        BoardTile.OnUnitPlaced += OnUnitPlaced;
    }

    private void OnDisable()
    {
        GameStateMachine.OnStateChanged -= IsKingCaptured;
        BoardTile.OnUnitPlaced -= OnUnitPlaced;
    }
    public override void StartTutorial()
    {
        _tutorialDeck.ResetDeck(_tutorialStartingDeck);
        GameStateMachine.Instance.ChangeState(new BuyUnitState(GameStateMachine.Instance, UnitFaction.White));
    }

    public void ContinueTutorial()
    {
        GameStateMachine.Instance.ChangeState(new MoveUnitState(GameStateMachine.Instance, UnitFaction.White));
        _previousPanel.SetActive(false);
        _nextPanel.SetActive(true);
    }

    private void OnUnitPlaced(Unit unit)
    {
        ContinueTutorial();
    }

    private void IsKingCaptured(GameState state)
    {
        if (state.Faction == UnitFaction.White)
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
        }
    }
}
