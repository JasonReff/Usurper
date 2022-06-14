using UnityEngine;

public abstract class SinglePlayerUpgrade : ScriptableObject
{
    public Sprite UpgradeSprite;

    private void OnEnable()
    {
        GameStateMachine.OnStateChanged += OnGameStart;
        GameStateMachine.OnStateChanged += OnGameEnd;
    }

    private void OnDisable()
    {
        GameStateMachine.OnStateChanged -= OnGameStart;
        GameStateMachine.OnStateChanged -= OnGameEnd;
    }

    private void OnGameStart(GameState state)
    {
        if (state.GetType() == typeof(StartGameState))
            StartUpgrade();
    }

    private void OnGameEnd(GameState state)
    {
        if (state.GetType() == typeof(PlayerWonState) || state.GetType() == typeof(PlayerLeftState))
            EndUpgrade();
    }

    public virtual void StartUpgrade()
    {

    }

    public virtual void EndUpgrade()
    {

    }
}
