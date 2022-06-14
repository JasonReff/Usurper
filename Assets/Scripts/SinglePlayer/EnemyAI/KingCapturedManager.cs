using System.Linq;
using UnityEngine;

public class KingCapturedManager : MonoBehaviour
{
    private void OnEnable()
    {
        KingUnit.OnKingCaptured += CheckForLivingKings;
    }

    private void OnDisable()
    {
        KingUnit.OnKingCaptured -= CheckForLivingKings;
    }

    private void CheckForLivingKings(KingUnit king)
    {
        if (!Board.Instance.PlayerUnits.Any(t => t.UnitData.IsKing))
        {
            GameStateMachine.Instance.ChangeState(new PlayerWonState(GameStateMachine.Instance, UnitFaction.Enemy));
        }
        else if (!Board.Instance.EnemyUnits.Any(t => t.UnitData.IsKing))
        {
            GameStateMachine.Instance.ChangeState(new PlayerWonState(GameStateMachine.Instance, UnitFaction.Player));
        }
    }
}