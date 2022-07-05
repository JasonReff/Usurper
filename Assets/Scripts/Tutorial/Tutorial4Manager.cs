using System.Collections;
using System.Linq;
using UnityEngine;

public class Tutorial4Manager : TutorialManager
{
    [SerializeField] private GameObject _sectionFailedPanel;
    [SerializeField] private PlayerDeck _tutorialDeck;
    [SerializeField] private StartingDeck _tutorialStartingDeck;

    private void OnEnable()
    {
        GameStateMachine.OnStateChanged += IsKingCaptured;
    }

    private void OnDisable()
    {
        GameStateMachine.OnStateChanged -= IsKingCaptured;
    }
    public override void StartTutorial()
    {
        _tutorialDeck.ResetDeck(_tutorialStartingDeck);
        GameStateMachine.Instance.ChangeState(new BuyUnitState(GameStateMachine.Instance, UnitFaction.Player));
    }

    private void IsKingCaptured(GameState state)
    {
        if (state.Faction == UnitFaction.Player)
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
}