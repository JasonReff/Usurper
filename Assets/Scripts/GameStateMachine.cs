using Photon.Pun;
using System;
using UnityEngine;

public class GameStateMachine : MonoBehaviourPunCallbacks
{
    public static GameStateMachine Instance;
    public static event Action<GameState> OnStateChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnDisable()
    {
        _currentState.EndState();
    }

    private void Start()
    {
        ChangeState(new StartGameState(this, UnitFaction.Player));
    }

    protected GameState _currentState;
    private void Update()
    {
        if (_currentState != null)
            _currentState.UpdateState();
    }
    public virtual void ChangeState(GameState newState)
    {
        if (_currentState != null)
            _currentState.EndState();
        _currentState = newState;
        _currentState.BeginState();
        OnStateChanged?.Invoke(_currentState);
    }

    public UnitFaction CurrentFaction()
    {
        return _currentState.Faction;
    }
}

public abstract class GameState
{
    protected GameStateMachine _stateMachine;
    public UnitFaction Faction;
    public GameState(GameStateMachine stateMachine, UnitFaction faction)
    {
        _stateMachine = stateMachine;
        Faction = faction;
    }

    public virtual void BeginState()
    {
        
    }

    public virtual void UpdateState()
    {

    }

    public virtual void EndState()
    {
        
    }

    protected UnitFaction GetOpposingFaction()
    {
        if (Faction == UnitFaction.Player)
            return UnitFaction.Enemy;
        else return UnitFaction.Player;
    }
}

public class MoveUnitState : GameState
{
    public static Action<GameState> OnMoveStateBegan;
    public static Action OnMoveStateEnded;
    public override void BeginState()
    {
        base.BeginState();
        Unit.OnUnitMoved += OnUnitMoved;
        OnMoveStateBegan?.Invoke(this);
    }

    public override void EndState()
    {
        base.EndState();
        Unit.OnUnitMoved -= OnUnitMoved;
        OnMoveStateEnded?.Invoke();
    }
    public MoveUnitState(GameStateMachine stateMachine, UnitFaction faction) : base(stateMachine, faction)
    {

    }

    private void OnUnitMoved()
    {
        _stateMachine.ChangeState(new BuyUnitState(_stateMachine, GetOpposingFaction()));
    }
}

public class BuyUnitState : GameState
{
    public static Action<GameState> OnBuyUnitStateEnded;
    public BuyUnitState(GameStateMachine stateMachine, UnitFaction faction) : base(stateMachine, faction)
    {

    }

    public override void BeginState()
    {
        base.BeginState();
        ShopManager.OnUnitPurchased += GoToMoveState;
        ShopManager.OnShopPhaseSkipped += GoToMoveState;
    }

    public override void EndState()
    {
        base.EndState();
        ShopManager.OnUnitPurchased -= GoToMoveState;
        ShopManager.OnShopPhaseSkipped -= GoToMoveState;
        OnBuyUnitStateEnded?.Invoke(this);
    }

    private void GoToMoveState()
    {
        _stateMachine.ChangeState(new MoveUnitState(_stateMachine, Faction));
    }

    private void GoToMoveState(Unit unit)
    {
        _stateMachine.ChangeState(new MoveUnitState(_stateMachine, Faction));
    }

}

public class StartGameState : GameState
{
    public StartGameState(GameStateMachine stateMachine, UnitFaction faction) : base(stateMachine, faction)
    {

    }

    public override void BeginState()
    {
        base.BeginState();
        OnGameStart?.Invoke();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        GoToShopState();
    }

    public static Action OnGameStart;
    private void GoToShopState()
    {
        _stateMachine.ChangeState(new BuyUnitState(_stateMachine, Faction));
    }
}

public class PlayerWonState : GameState
{
    public PlayerWonState(GameStateMachine stateMachine, UnitFaction faction) : base(stateMachine, faction)
    {

    }

    public override void BeginState()
    {
        base.BeginState();
    }
}