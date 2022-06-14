using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/StartingGold")]
public class StartingGoldUpgrade : SinglePlayerUpgrade
{
    [SerializeField] private PlayerDeck _playerDeck;
    [SerializeField] private int _goldIncrease;
    private int _defaultGold;
    public override void StartUpgrade()
    {
        _defaultGold = _playerDeck.StartingGold;
        _playerDeck.StartingGold += _goldIncrease;
    }

    public override void EndUpgrade()
    {
        _playerDeck.StartingGold = _defaultGold;
    }
}
