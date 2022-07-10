using UnityEngine;

public class CheatButton : MonoBehaviour
{
    public void WinGame()
    {
        GameStateMachine.Instance.ChangeState(new PlayerWonState(GameStateMachine.Instance, UnitFaction.White));
    }
}