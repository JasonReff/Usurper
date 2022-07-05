using UnityEngine;

public class Tutorial2Manager : TutorialManager
{
    [SerializeField] private PlayerDeck _tutorialDeck;
    [SerializeField] private StartingDeck _tutorialStartingDeck;
    private void OnEnable()
    {
        BoardTile.OnUnitPlaced += OnUnitPlaced;
    }

    private void OnDisable()
    {
        BoardTile.OnUnitPlaced -= OnUnitPlaced;
    }

    private void OnUnitPlaced(Unit unit)
    {
        TutorialComplete();
    }

    public override void StartTutorial()
    {
        _tutorialDeck.ResetDeck(_tutorialStartingDeck);
        GameStateMachine.Instance.ChangeState(new BuyUnitState(GameStateMachine.Instance, UnitFaction.Player));
    }
}
