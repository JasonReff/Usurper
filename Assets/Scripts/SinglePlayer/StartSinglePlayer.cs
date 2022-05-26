using UnityEngine;

public class StartSinglePlayer : MonoBehaviour
{
    [SerializeField] private SceneLoader _sceneLoader;
    [SerializeField] private StartingDeck _singlePlayerDeck;
    [SerializeField] private PlayerDeck _playerDeck;
    [SerializeField] private EnemyDeckGenerator _enemyDeckGenerator;
    [SerializeField] private SinglePlayerStats _stats;

    public void StartGame()
    {
        SetupDeck();
        _enemyDeckGenerator.SetDeck(1);
        _stats.ResetStats();
        _sceneLoader.LoadScene("SinglePlayerGame");

    }

    private void SetupDeck()
    {
        _playerDeck.ResetDeck(_singlePlayerDeck);
    }
}