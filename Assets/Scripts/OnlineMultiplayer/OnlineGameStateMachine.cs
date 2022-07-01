using Photon.Pun;
using Photon.Realtime;
using System;

public class OnlineGameStateMachine : GameStateMachine
{
    public static Action OnOnlineStateChanged;

    protected override void Start()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            ChangeState(new StartGameState(this, UnitFaction.Player));
        }
        else
        {
            base.ChangeState(new WaitingState(this, UnitFaction.Player));
        }
    }
    public override void ChangeState(GameState newState)
    {
        base.ChangeState(newState);
        this.photonView.RPC("OnStateChangedCallback", RpcTarget.Others, new object[] { newState.GetType().ToString(), newState.Faction });
    }

    [PunRPC]
    private void OnStateChangedCallback(string stateName, UnitFaction faction)
    {
        if (stateName == _currentState.GetType().ToString() && _currentState.Faction == faction)
            return;
        switch (stateName)
        {
            case "StartGameState":
                base.ChangeState(new StartGameState(Instance, faction));
                break;
            case "BuyUnitState":
                base.ChangeState(new BuyUnitState(Instance, faction));
                break;
            case "MoveUnitState":
                base.ChangeState(new MoveUnitState(Instance, faction));
                break;
            case "PlayerWonState":
                base.ChangeState(new PlayerWonState(Instance, faction));
                break;
        }
        OnOnlineStateChanged?.Invoke();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.ChangeState(new PlayerLeftState(Instance, UnitFaction.Player));
    }
}

public class PlayerLeftState : GameState
{
    public PlayerLeftState(GameStateMachine stateMachine, UnitFaction faction) : base(stateMachine, faction)
    {

    }

    public override void BeginState()
    {
        base.BeginState();
        _stateMachine.SetGameEnded();
    }
}

public class WaitingState : GameState
{
    public WaitingState(GameStateMachine stateMachine, UnitFaction faction) : base(stateMachine, faction)
    {

    }
}