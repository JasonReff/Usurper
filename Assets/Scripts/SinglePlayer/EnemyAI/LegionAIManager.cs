using System.Collections;
using UnityEngine;

public class LegionAIManager : SpecialEnemyManager
{
    protected override IEnumerator PlaceUnitCoroutine()
    {
        yield return new WaitForSeconds(1);
        GetBestUnitPlacement();
        PlaceUnit();
        yield return new WaitForSeconds(1);
        SetupShop();
        GetBestUnitPlacement();
        PlaceUnit();
        yield return new WaitForSeconds(1);
        GameStateMachine.Instance.ChangeState(new MoveUnitState(GameStateMachine.Instance, _faction));
    }
}
