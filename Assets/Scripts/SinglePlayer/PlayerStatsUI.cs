using TMPro;
using UnityEngine;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _onlineStatsTextbox, _singlePlayerStatsTextbox;
    [SerializeField] private PlayerStats _stats;

    public void ResetStats()
    {
        _stats.ResetStats();
    }

    public void DisplayStats()
    {
        _onlineStatsTextbox.text = $"Online Games (Win/Loss/Tie): {_stats.OnlineGamesWon} / {_stats.OnlineGamesLost} / {_stats.OnlineGamesDrawn}";
        _singlePlayerStatsTextbox.text = $"Single Player Runs (Win/Loss): {_stats.SinglePlayerRunsWon} / {_stats.SinglePlayerRunsLost}";
    }
}