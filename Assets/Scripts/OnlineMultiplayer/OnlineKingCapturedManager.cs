using Photon.Pun;
using System.Linq;
using UnityEngine;

public class OnlineKingCapturedManager : KingCapturedManager
{
    public UnitFaction Faction;
    [SerializeField] private PlayerStats _stats;

    protected override void OnEnable()
    {
        base.OnEnable();
        OnlinePlayerManager.OnPlayerOneFactionAssigned += SetFaction;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        OnlinePlayerManager.OnPlayerOneFactionAssigned -= SetFaction;
    }

    protected override void CheckForLivingKings(Unit unit)
    {
        if (!Board.Instance.PlayerUnits.Any(t => t.UnitData.IsKing))
        {
            GameStateMachine.Instance.ChangeState(new PlayerWonState(GameStateMachine.Instance, UnitFaction.Black));
            AdjustStats(unit.Faction);

        }
        else if (!Board.Instance.EnemyUnits.Any(t => t.UnitData.IsKing))
        {
            GameStateMachine.Instance.ChangeState(new PlayerWonState(GameStateMachine.Instance, UnitFaction.White));
            AdjustStats(unit.Faction);
        }
    }

    private void AdjustStats(UnitFaction kingFaction)
    {
        if (kingFaction == Faction)
            _stats.OnlineGamesLost++;
        else _stats.OnlineGamesWon++;
    }

    private void SetFaction(UnitFaction faction)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
            Faction = faction;
        else Faction = faction.GetOpposite();
    }
}