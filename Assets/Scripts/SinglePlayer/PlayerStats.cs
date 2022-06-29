using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [SerializeField] private int _onlineGamesWon, _onlineGamesLost, _onlineGamesDrawn, _singlePlayerRunsWon, _singlePlayerRunsLost;

    public int OnlineGamesWon { get => PlayerPrefs.GetInt("OnlineGamesWon"); set { _onlineGamesWon = value; PlayerPrefs.SetInt("OnlineGamesWon", _onlineGamesWon); } }
    public int OnlineGamesLost { get => PlayerPrefs.GetInt("OnlineGamesLost"); set { _onlineGamesLost = value; PlayerPrefs.SetInt("OnlineGamesLost", _onlineGamesLost); } }
    public int OnlineGamesDrawn { get => PlayerPrefs.GetInt("OnlineGamesDrawn"); set { _onlineGamesDrawn = value; PlayerPrefs.SetInt("OnlineGamesDrawn", _onlineGamesDrawn); } }
    public int SinglePlayerRunsWon { get => PlayerPrefs.GetInt("SinglePlayerRunsWon"); set { _singlePlayerRunsWon = value; PlayerPrefs.SetInt("SinglePlayerRunsWon", _singlePlayerRunsWon); } }
    public int SinglePlayerRunsLost { get => PlayerPrefs.GetInt("SinglePlayerRunsLost"); set { _singlePlayerRunsLost = value; PlayerPrefs.SetInt("SinglePlayerRunsLost", _singlePlayerRunsLost); } }

    public void ResetStats()
    {
        OnlineGamesWon = 0;
        OnlineGamesLost = 0;
        OnlineGamesDrawn = 0;
        SinglePlayerRunsWon = 0;
        SinglePlayerRunsLost = 0;
    }
}
