using Photon.Pun;
using System.Linq;
using UnityEngine;

public class KingCapturedManager : MonoBehaviourPunCallbacks
{
    protected virtual void OnEnable()
    {
        Board.OnUnitRemoved += CheckForLivingKings;
    }

    protected virtual void OnDisable()
    {
        Board.OnUnitRemoved -= CheckForLivingKings;
    }

    protected virtual void CheckForLivingKings(Unit unit)
    {
        if (!Board.Instance.PlayerUnits.Any(t => t.UnitData.IsKing))
        {
            GameStateMachine.Instance.ChangeState(new PlayerWonState(GameStateMachine.Instance, UnitFaction.Black));
        }
        else if (!Board.Instance.EnemyUnits.Any(t => t.UnitData.IsKing))
        {
            GameStateMachine.Instance.ChangeState(new PlayerWonState(GameStateMachine.Instance, UnitFaction.White));
        }
    }
}
