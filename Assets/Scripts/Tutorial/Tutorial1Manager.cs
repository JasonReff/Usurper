public class Tutorial1Manager : TutorialManager
{

    private void OnEnable()
    {
        Unit.OnUnitMoved += TutorialComplete;
    }

    private void OnDisable()
    {
        Unit.OnUnitMoved -= TutorialComplete;
    }

    public override void StartTutorial()
    {
        GameStateMachine.Instance.ChangeState(new MoveUnitState(GameStateMachine.Instance, UnitFaction.Player));
    }
}
